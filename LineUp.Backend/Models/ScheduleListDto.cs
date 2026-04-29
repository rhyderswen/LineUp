namespace LineUp.Backend.Models;

/// <summary>
/// DTO for the list of schedules.
/// </summary>
public class ScheduleListDto
{
    /// <summary>
    /// The schedule's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The schedule's GUID.
    /// </summary>
    public required Guid Guid { get; set; }

    /// <summary>
    /// The number of respondents assigned to the schedule.
    /// </summary>
    public required int Respondents { get; set; }

    /// <summary>
    /// The number of availabilities associated with the schedule.
    /// </summary>
    public required bool IsGenerated { get; set; }
}
