namespace LineUp.Backend.Models;

public class ScheduleDto
{
    public int? Id { get; set; }
    public Guid? Guid { get; init; }

    public required DateOnly[] DateCoverage { get; set; }

    public required bool[] WeekdayCoverage { get; set; } //which weekdays will need to be scheduled (7-variable array, [0] = Sunday).

    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }

    //TimeOnly is an object that basically reads like a clock: e.g. 11:49:50.00

    public string[,]? ShiftAssignment { get; set; } //an array that lists avail IDs.

    //Indexing: For shiftAssignment[i][j],

    public required Preferences Preferences { get; set; } = new();
}
