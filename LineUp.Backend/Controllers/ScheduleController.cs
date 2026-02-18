using LineUp.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineUp.Backend.Controllers;

[Route("api/schedule")]
[ApiController]
public class ScheduleController : ControllerBase
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

    [HttpGet("dateCoverage")]
    public async Task<IActionResult> GetDateCoverage()
    {
        LineUpContext db = new LineUpContext();
        Schedule result = await db.FindAsync<Schedule>();
        if (result != null)
            return Ok(new { Message = "Querying a Date Coverage" + result.ID });
        else
            return NotFound(new { Message = "No Schedule Found" });
    }

    [HttpPost("dateCoverage")]
    public IActionResult PostDateCoverage()
    {
        LineUpContext db = new LineUpContext();
        db.Add(
            new Schedule()
            {
                // DateCoverage = new DateTime[];
            }
        );
        return Ok(new { Message = "Posted new date coverage" });
    }
}
