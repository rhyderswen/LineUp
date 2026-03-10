using System.Security.Claims;
using LineUp.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace LineUp.Backend.Controllers;

[Route("api/schedule")]
[ApiController]
public class ScheduleController(LineUpContext context) : ControllerBase
{
    [HttpGet("{guid:guid}")]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> GetSchedule(Guid guid)
    {
        var schedule = await context
            .Schedules.Include(s => s.SchedulePreferences)
            .FirstOrDefaultAsync(s => s.Guid == guid);
        if (schedule == null)
            return NotFound();

        var isAuthenticatedAndCanAccessSchedule =
            User.Identity?.IsAuthenticated == true
            && User.FindFirstValue(ClaimTypes.NameIdentifier) == schedule.Auth0UserId;

        if (isAuthenticatedAndCanAccessSchedule)
        {
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
            };
            return Ok(dto);
        }
        else
        {
            var dto = new GetScheduleUnauthenticatedDto
            {
                Name = schedule.Name,
                DateCoverage = schedule.DateCoverage,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Form = schedule.Form,
                ShiftAssignments = schedule.ShiftAssignments,
                SchedulePreferences = schedule.SchedulePreferences,
            };

            return Ok(dto);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetSchedules()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        List<Schedule> result = await context
            .Schedules.Where(s => s.Auth0UserId == userId)
            .ToListAsync();

        return Ok(result);
    }

    [HttpDelete("{guid:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteSchedule(Guid guid)
    {
        var scheduleToDelete = await context.Schedules.FirstOrDefaultAsync(s => s.Guid == guid);
        if (scheduleToDelete == null)
            return NotFound();
        if (scheduleToDelete.Auth0UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            return Unauthorized();
        context.Schedules.Remove(scheduleToDelete);
        await context.SaveChangesAsync();
        return NoContent();
    }

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

    [HttpGet("{guid:guid}/createAvailability")] //Creates a new availability using this guid for the parent schedule and generating a new guid for the Availability.
    public IActionResult CreateAvailability(Guid guid)
    {
        Schedule? schedule = context.Schedules.FirstOrDefault<Schedule>(s => s.Guid == guid);
        if (schedule == null)
        {
            return NotFound();
        }
        Availability availability = new Availability { UserName = "", Schedule = schedule };
        context.Availabilities.Add(availability);
        context.SaveChanges();
        return Ok(availability.Guid);
    }
}
