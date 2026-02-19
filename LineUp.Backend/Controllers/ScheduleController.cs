using LineUp.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LineUp.Backend.Controllers;

[Route("api/schedule")]
[ApiController]
public class ScheduleController(LineUpContext context) : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok(
            new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this.",
            }
        );
    }

    // This is a helper action. It allows you to easily view all the claims of the token.
    [HttpGet("claims")]
    public IActionResult Claims()
    {
        return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    }

    [HttpGet("id/{id:guid}")]
    public async Task<IActionResult> GetDateCoverage(int id)
    {
        var result = await context.FindAsync<Schedule>(id);
        if (result != null)
            return Ok(result);
        return NotFound();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostDateCoverage([FromBody] ScheduleDto schedule)
    {
        var scheduleToInsert = new Schedule
        {
            Auth0UserId = User.FindFirst("sub")!.Value,
            Guid = Guid.NewGuid(),
            DateCoverage = schedule.DateCoverage,
            WeekdayCoverage = schedule.WeekdayCoverage,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            Preferences = schedule.Preferences,
        };

        context.Schedules.Add(scheduleToInsert);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetDateCoverage),
            new { guid = scheduleToInsert.Guid },
            schedule
        );
    }
}
