public class Schedule
{
    public String ID = "";

    public String authToken;

    public DateOnly[] dateCoverage;

    public bool[] weekdayCoverage; //which weekdays will need to be scheduled (7-variable array, [0] = Sunday).

    public TimeOnly[] hourCoverage; //A two-object array, {Start, End}.
    //TimeOnly is an object that basically reads like a clock: e.g. 11:49:50.00

    public String[][] shiftAssignment; //an array that lists avail IDs.
    //Indexing: For shiftAssignment[i][j],

    public Object[] preferences;
}
