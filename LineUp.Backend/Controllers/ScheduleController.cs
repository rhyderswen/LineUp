using System.Security.Claims;
using LineUp.Backend.Models;
using LineUp.Backend.Services;
using LineUp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LineUp.Backend.Controllers;

/// <summary>
/// Controller for managing schedules.
/// </summary>
/// <param name="context"></param>
/// <param name="emailService"></param>
[Route("api/schedule")]
[ApiController]
public class ScheduleController(LineUpContext context, IEmailService emailService) : ControllerBase
{
    /// <summary>
    /// Gets a schedule by its guid.
    /// Requires authentication.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>HTTP 200 OK with schedule details, otherwise HTTP 404 Not Found.</returns>
    [HttpGet("{guid:guid}/details")]
    [Authorize]
    public async Task<IActionResult> GetScheduleAuthenticated(Guid guid)
    {
        var schedule = await context
            .Schedules.Include(s => s.SchedulePreferences)
            .Include(schedule => schedule.Form)
            .Include(schedule => schedule.ShiftAssignments)
            .FirstOrDefaultAsync(s => s.Guid == guid);
        if (schedule == null)
            return NotFound();

        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != schedule.Auth0UserId)
            return Unauthorized();
        List<Availability> availabilities = await context
            .Availabilities.Where(availability => availability.Schedule.Guid == guid)
            .ToListAsync();
        var dto = new GetScheduleAuthenticatedDto
        {
            Name = schedule.Name,
            DateCoverage = schedule.DateCoverage,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            Form = schedule.Form,
            ShiftAssignments = schedule.ShiftAssignments,
            SchedulePreferences = schedule.SchedulePreferences,
            Availabilities = availabilities,
            LatestEmailsSent = schedule.LatestEmailsSent,
        };
        return Ok(dto);
    }

    /// <summary>
    /// Gets a schedule by its guid.
    /// </summary>
    /// <param name="guid">The Schedule's GUID.</param>
    /// <returns>HTTP 200 OK with schedule details, otherwise HTTP 404 Not Found.</returns>
    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetSchedule(Guid guid)
    {
        var schedule = await context
            .Schedules.Include(s => s.SchedulePreferences)
            .Include(schedule => schedule.Form)
            .Include(schedule => schedule.ShiftAssignments)
            .FirstOrDefaultAsync(s => s.Guid == guid);
        if (schedule == null)
            return NotFound();
        if (schedule.ShiftAssignments != null && schedule.ShiftAssignments.Count != 0)
        {
            foreach (var shiftAssignment in schedule.ShiftAssignments)
            {
                await context.Entry(shiftAssignment).Reference(sa => sa.Availability).LoadAsync();
            }
        }

        var availabilityCount = context.Availabilities.Count(availability =>
            availability.Schedule.Guid == guid
        );

        var dto = new GetScheduleUnauthenticatedDto
        {
            Name = schedule.Name,
            DateCoverage = schedule.DateCoverage,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            Form = schedule.Form,
            ShiftAssignments = schedule.ShiftAssignments,
            SchedulePreferences = schedule.SchedulePreferences,
            AvailabilityCount = availabilityCount,
        };

        return Ok(dto);
    }

    /// <summary>
    /// Gets all schedules for the authenticated user.
    /// </summary>
    /// <returns>HTTP 200 OK with list of schedules, otherwise HTTP 401 Unauthorized.</returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetSchedules()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        List<ScheduleListDto> result = await context
            .Schedules.Where(s => s.Auth0UserId == userId)
            .OrderByDescending(s => s.Id)
            .Include(s => s.ShiftAssignments)
            .Select(s => new ScheduleListDto
            {
                Name = s.Name,
                Guid = s.Guid,
                Respondents = context.Availabilities.Count(a => a.Schedule.Id == s.Id),
                IsGenerated = s.ShiftAssignments != null && s.ShiftAssignments.Count != 0,
            })
            .ToListAsync();

        return Ok(result);
    }

    /// <summary>
    /// Deletes a schedule.
    /// </summary>
    /// <param name="guid">The GUID of the schedule you want to delete.</param>
    /// <returns>HTTP 204 No Content on successful deletion, otherwise HTTP 401 Unauthorized or HTTP 404 Not Found.</returns>
    [HttpDelete("{guid:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteSchedule(Guid guid)
    {
        var scheduleToDelete = await context
            .Schedules.Include(schedule => schedule.ShiftAssignments)
            .FirstOrDefaultAsync(s => s.Guid == guid);
        if (scheduleToDelete == null)
            return NotFound();
        if (scheduleToDelete.Auth0UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            return Unauthorized();

        IQueryable<ShiftAssignment> shiftAssignments = context.ShiftAssignments.Where(sa =>
            sa.ScheduleId == scheduleToDelete.Id
        );
        context.ShiftAssignments.RemoveRange(shiftAssignments);

        context.Schedules.Remove(scheduleToDelete);
        await context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Updates a schedule.
    /// </summary>
    /// <param name="guid">The GUID of the schedule you want to update.</param>
    /// <param name="schedule">The updated schedule details.</param>
    /// <returns>HTTP 204 No Content on successful update, otherwise HTTP 401 Unauthorized or HTTP 404 Not Found.</returns>
    [HttpPatch("{guid:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateSchedule(
        Guid guid,
        [FromBody] ScheduleUpdateDto schedule
    )
    {
        var scheduleToUpdate = await context.Schedules.FirstOrDefaultAsync(s => s.Guid == guid);
        if (scheduleToUpdate == null)
            return NotFound();
        if (scheduleToUpdate.Auth0UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            return Unauthorized();
        if (schedule.Name != null)
            scheduleToUpdate.Name = schedule.Name;
        if (schedule.SchedulePreferences != null)
            scheduleToUpdate.SchedulePreferences = schedule.SchedulePreferences;
        if (schedule.ShiftAssignments != null)
            scheduleToUpdate.ShiftAssignments = schedule.ShiftAssignments;
        await context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Creates a new schedule.
    /// </summary>
    /// <param name="schedule">The details of the schedule to create.</param>
    /// <returns>HTTP 201 Created with the created schedule's details, otherwise HTTP 401 Unauthorized.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSchedule([FromBody] ScheduleDto schedule)
    {
        var scheduleToInsert = new Schedule
        {
            Auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
            Guid = Guid.NewGuid(),
            DateCoverage = schedule.DateCoverage,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            SchedulePreferences = schedule.SchedulePreferences,
            Name = schedule.Name,
        };

        context.Schedules.Add(scheduleToInsert);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetSchedule),
            new { guid = scheduleToInsert.Guid },
            scheduleToInsert
        );
    }

    /// <summary>
    /// Generates a schedule.
    /// </summary>
    /// <param name="guid">The GUID of the schedule to generate based on users' availabilities.</param>
    /// <param name="random">Whether to randomize the schedule or not.</param>
    /// <returns>HTTP 201 Created with the generated schedule's details, otherwise HTTP 401 Unauthorized or HTTP 404 Not Found.</returns>
    [HttpGet("{guid:guid}/generateSchedule")]
    [Authorize]
    public async Task<IActionResult> GenerateSchedule(Guid guid, [FromQuery] bool random = true)
    {
        var schedule = await context
            .Schedules.Include(schedule => schedule.SchedulePreferences)
            .Include(schedule => schedule.ShiftAssignments)
            .FirstOrDefaultAsync(s => s.Guid == guid);
        if (schedule == null)
            return NotFound();
        List<Availability> availabilities = await context
            .Availabilities.Include(a => a.Schedule)
                .ThenInclude(s => s.ShiftAssignments)
            .Where(a => a.Schedule == schedule)
            .ToListAsync();
        if (schedule.Auth0UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            return Unauthorized();

        var updated = await context.ShiftAssignments.AnyAsync(shiftAssignment =>
            shiftAssignment.ScheduleId == schedule.Id
        );

        var result = Scheduler.Scheduler.RunScheduler(
            schedule,
            availabilities,
            schedule.SchedulePreferences,
            random
        );

        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            await context
                .ShiftAssignments.Where(shiftAssignment =>
                    shiftAssignment.ScheduleId == schedule.Id
                )
                .ExecuteDeleteAsync();

            if (result.Assignments != null)
                await context.ShiftAssignments.AddRangeAsync(result.Assignments);

            schedule.LatestEmailsSent = false;

            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        });

        schedule.ShiftAssignments = result.Assignments;

        availabilities = await context
            .Availabilities.Where(a => a.Schedule == schedule)
            .ToListAsync();

        return Ok(result);
    }

    /// <summary>
    /// Sends emails to all users in a schedule.
    /// </summary>
    /// <param name="scheduleGuid">The GUID of the schedule to send emails for.</param>
    /// <returns>HTTP 200 OK, otherwise HTTP 401 Unauthorized or HTTP 404 Not Found.</returns>
    [HttpPost("{scheduleGuid:Guid}/sendEmails")]
    [Authorize]
    public async Task<IActionResult> SendEmails(Guid scheduleGuid)
    {
        var schedule = await context
            .Schedules.Include(schedule => schedule.SchedulePreferences)
            .Include(schedule => schedule.ShiftAssignments)
            .FirstOrDefaultAsync(s => s.Guid == scheduleGuid);
        if (schedule == null)
            return NotFound();
        List<Availability> availabilities = await context
            .Availabilities.Where(a => a.Schedule == schedule)
            .ToListAsync();

        foreach (var availability in availabilities)
        {
            availability.Schedule = schedule;
            await emailService.SendShiftAssignmentEmail(
                (schedule.ShiftAssignments ?? []).Count != 0,
                availability
            );
        }

        schedule.LatestEmailsSent = true;
        await context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Creates an availability for a schedule.
    /// </summary>
    /// <param name="scheduleGuid">The GUID of the schedule to create an availability for.</param>
    /// <param name="availability">The details of the availability to create.</param>
    /// <returns>HTTP 200 OK, otherwise HTTP 401 Unauthorized or HTTP 404 Not Found.</returns>
    [HttpPost("{scheduleGuid:Guid}/createAvailability")]
    public async Task<IActionResult> CreateAvailability(
        Guid scheduleGuid,
        [FromBody] AvailabilityCreateDto availability
    )
    {
        var schedule = await context.Schedules.FirstOrDefaultAsync(s => s.Guid == scheduleGuid);
        if (schedule == null)
        {
            return NotFound();
        }

        if (availability.UserName.Trim().Length == 0)
        {
            return BadRequest("User name cannot be empty");
        }

        if (
            context.Availabilities.Any(a =>
                a.UserName == availability.UserName && a.Schedule.Guid == scheduleGuid
            )
        )
        {
            return Conflict("Conflicting user name!");
        }

        if (
            await context.Availabilities.AnyAsync(a =>
                a.UserEmail == availability.UserEmail && a.Schedule.Guid == scheduleGuid
            )
        )
        {
            return UnprocessableEntity("Email already exists in this schedule!");
        }

        var availabilityToInsert = new Availability
        {
            Guid = Guid.NewGuid(),
            Schedule = schedule,
            AvailabilitySlots = availability.AvailabilitySlots,
            UserName = availability.UserName,
            UserEmail = availability.UserEmail,
            Preferences = availability.Preferences,
            FormAnswers = availability.FormAnswers,
        };

        context.Availabilities.Add(availabilityToInsert);
        await context.SaveChangesAsync();

        await emailService.SendAvailabilityConfirmationEmail(false, availabilityToInsert);

        return CreatedAtAction(
            nameof(AvailabilityController.GetAvailability),
            "Availability",
            new { guid = availabilityToInsert.Guid },
            availabilityToInsert
        );
    }
}
