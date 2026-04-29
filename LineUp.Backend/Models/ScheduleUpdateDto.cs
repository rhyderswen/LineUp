using LineUp.Core.Models;

namespace LineUp.Backend.Models;

/// <summary>
/// DTO for updating a schedule.
/// </summary>
public class ScheduleUpdateDto
{
    /// <summary>
    /// The ID of the schedule.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// The GUID of the schedule.
    /// </summary>
    public Guid? Guid { get; init; }

    /// <summary>
    /// The shift assignments associated with the schedule.
    /// </summary>
    public ICollection<ShiftAssignment>? ShiftAssignments { get; set; } =
        new List<ShiftAssignment>();

    /// <summary>
    /// The schedule's preferences.
    /// </summary>
    public SchedulePreferences? SchedulePreferences { get; set; } = new();

    /// <summary>
    /// The name of the schedule.
    /// </summary>
    public string? Name { get; set; }
}
