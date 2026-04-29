using LineUp.Backend.Models;
using LineUp.Backend.Services;
using LineUp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LineUp.Backend.Controllers;

/// <summary>
/// Controller for managing availabilities.
/// </summary>
/// <param name="context"></param>
/// <param name="emailService"></param>
[Route("api/availability")]
[ApiController]
public class AvailabilityController(LineUpContext context, IEmailService emailService)
    : ControllerBase
{
    /// <summary>
    /// Checks if an availability with the given guid exists.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>HTTP 200 OK if found, otherwise HTTP 404 Not Found.</returns>
    [HttpGet("{guid:guid}/exists")]
    public IActionResult Exists(Guid guid)
    {
        if (context.Availabilities.Any(a => a.Guid == guid))
        {
            return Ok();
        }

        return NotFound();
    }

    /// <summary>
    /// Gets an availability by its guid.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>HTTP 200 OK with availability details, otherwise HTTP 404 Not Found.</returns>
    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetAvailability(Guid guid)
    {
        var result = await context
            .Availabilities.Include(a => a.Schedule)
            .FirstOrDefaultAsync(a => a.Guid == guid);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Edits an availability.
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="availability"></param>
    /// <returns>HTTP 204 No Content if successful, HTTP 404 Not Found if availability not found, HTTP 403 Forbidden if shift assignments exist.</returns>
    [HttpPatch("{guid:guid}/edit")]
    public async Task<IActionResult> EditAvailability(
        Guid guid,
        [FromBody] AvailabilityUpdateDto availability
    )
    {
        var availabilityToUpdate = await context
            .Availabilities.Include(a => a.Schedule)
            .FirstOrDefaultAsync(a => a.Guid == guid);
        if (availabilityToUpdate == null)
            return NotFound();

        if (
            await context.ShiftAssignments.AnyAsync(sa =>
                sa.Availability != null && sa.Availability.Id == availabilityToUpdate.Id
            )
        )
        {
            return Forbid();
        }

        availabilityToUpdate.UserName = availability.UserName ?? availabilityToUpdate.UserName;
        availabilityToUpdate.UserEmail = availability.UserEmail ?? availabilityToUpdate.UserEmail;
        availabilityToUpdate.AvailabilitySlots =
            availability.AvailabilitySlots ?? availabilityToUpdate.AvailabilitySlots;

        await emailService.SendAvailabilityConfirmationEmail(true, availabilityToUpdate);

        await context.SaveChangesAsync();

        return NoContent();
    }
}
