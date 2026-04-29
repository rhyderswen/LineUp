using System.ComponentModel.DataAnnotations;
using LineUp.Core.Models;
using LineUp.Core.Models.Forms;

namespace LineUp.Backend.Models;

/// <summary>
/// DTO for getting a schedule while unauthenticated.
/// </summary>
public class GetScheduleUnauthenticatedDto
{
    /// <summary>
    /// The schedule's name.
    /// </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    /// <summary>
    /// The schedule's date coverage.
    /// </summary>
    public required DateOnly[] DateCoverage { get; set; }

    /// <summary>
    /// The schedule's start time.
    /// </summary>
    public required TimeOnly StartTime { get; set; }

    /// <summary>
    /// The schedule's end time.
    /// </summary>
    public required TimeOnly EndTime { get; set; }

    /// <summary>
    /// The <seealso cref="Form"/> associated with the schedule.
    /// </summary>
    public Form? Form { get; set; }
    public int? FormId { get; set; }

    /// <summary>
    /// The shift assignments associated with the schedule.
    /// </summary>
    public ICollection<ShiftAssignment>? ShiftAssignments { get; set; } =
        new List<ShiftAssignment>();

    /// <summary>
    /// The schedule's preferences.
    /// </summary>
    public required SchedulePreferences SchedulePreferences { get; set; } = new();

    /// <summary>
    /// The number of availabilities associated with the schedule.
    /// </summary>
    public required int AvailabilityCount { get; set; }
}
