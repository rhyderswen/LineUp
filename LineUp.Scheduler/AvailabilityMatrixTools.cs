using LineUp.Core.Models;

namespace LineUp.Scheduler;

static class AvailabilityMatrixTools
{
    /// <summary>
    /// Generates a matrix of the correct size to store the availability of each slot.
    /// </summary>
    /// <seealso cref="GenerateMatrixPointerHashSet"> See GenerateMatrixPointerHashSet for a helper method to help you get the indices to set for each time.</seealso>
    /// <param name="schedule">A Schedule object. Make sure to Include the SchedulePreferences in your database query.</param>
    /// <returns>A matrix of the correct size to store the availability of each slot.</returns>
    public static int[,] GenerateMatrixFromSchedule(Schedule schedule)
    {
        return new int[
            schedule.DateCoverage.Length,
            (int)
                Math.Floor(
                    (schedule.EndTime - schedule.StartTime).TotalMinutes
                        / schedule.SchedulePreferences.MinutesPerSlot
                )
        ];
    }

    public static Dictionary<TimeOnly, int> GenerateMatrixPointerHashSet(Schedule schedule)
    {
        Dictionary<TimeOnly, int> result = new();
        var index = 0;
        for (
            var i = schedule.StartTime;
            i < schedule.EndTime;
            i = i.AddMinutes(schedule.SchedulePreferences.MinutesPerSlot)
        )
        {
            result.Add(i, index);
            index++;
        }
        return result;
    }
}
