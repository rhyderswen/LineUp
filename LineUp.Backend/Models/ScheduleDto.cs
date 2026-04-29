using LineUp.Core.Models;

namespace LineUp.Backend.Models;

/// <summary>
/// DTO for a schedule.
/// </summary>
public class ScheduleDto
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
    /// The shift assignments associated with the schedule.
    /// </summary>
    public ICollection<ShiftAssignment> ShiftAssignments { get; set; } =
        new List<ShiftAssignment>();

    /// <summary>
    /// The schedule's preferences.
    /// </summary>
    public required SchedulePreferences SchedulePreferences { get; set; } = new();

    /// <summary>
    /// The name of the schedule.
    /// </summary>
    public required string Name { get; set; }
}
