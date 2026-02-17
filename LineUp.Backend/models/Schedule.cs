using System;

namespace LineUp.Backend.Models;

public class Schedule
{
    public string ID { get; set; }

    public string authToken { get; set; }

    public DateOnly[] dateCoverage { get; set; }

    public bool[] weekdayCoverage { get; set; } //which weekdays will need to be scheduled (7-variable array, [0] = Sunday).

    public TimeOnly[] hourCoverage { get; set; } //A two-object array, {Start, End}.

    //TimeOnly is an object that basically reads like a clock: e.g. 11:49:50.00

    public string[,] shiftAssignment { get; set; } //an array that lists avail IDs.

    //Indexing: For shiftAssignment[i][j],

    public Preferences preferences { get; set; }
}
