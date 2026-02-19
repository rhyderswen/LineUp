using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LineUp.Backend.Models;

[Index(nameof(Auth0UserId), nameof(Guid))]
public class Schedule
{
    public int Id { get; set; }
    public Guid Guid { get; init; } = Guid.NewGuid();

    [MaxLength(256)]
    public required string Auth0UserId { get; set; }

    public required DateOnly[] DateCoverage { get; set; }

    public required bool[] WeekdayCoverage { get; set; } //which weekdays will need to be scheduled (7-variable array, [0] = Sunday).

    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }

    //TimeOnly is an object that basically reads like a clock: e.g. 11:49:50.00

    public ICollection<ShiftAssignment> ShiftAssignments { get; set; } =
        new List<ShiftAssignment>();

    //Indexing: For shiftAssignment[i][j],

    public required Preferences Preferences { get; set; } = new();
}
