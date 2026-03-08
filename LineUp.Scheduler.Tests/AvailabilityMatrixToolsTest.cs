using FluentAssertions;
using JetBrains.Annotations;
using LineUp.Core.Models;
using LineUp.Scheduler;

namespace LineUp.Scheduler.Tests;

[TestSubject(typeof(AvailabilityMatrixTools))]
public class AvailabilityMatrixToolsTest
{
    private readonly Schedule schedule = new()
    {
        Id = 0,
        Guid = default,
        Auth0UserId = "test-test-123-lineup-test",
        Name = null,
        DateCoverage =
        [
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
        ],
        StartTime = TimeOnly.FromDateTime(DateTime.Today),
        EndTime = TimeOnly.FromDateTime(DateTime.Today.AddHours(1)),
        Form = null,
        FormId = null,
        ShiftAssignments = [],
        SchedulePreferences = new SchedulePreferences
        {
            Id = default,
            MinutesPerSlot = 15,
            ShiftIntervals = 0,
            UsersPerShift = 0,
            MaximumShiftDurationMinutes = 0,
            MaximumShiftsPerWorker = 0
        },
    };

    [Fact]
    public void GenerateMatrixFromSchedule_IsCorrectSize()
    {
        // Act
        var result = AvailabilityMatrixTools.GenerateMatrixFromSchedule(schedule);
        
        // Assert
        result.Should().NotBeNull();
        result.GetLength(0).Should().Be(3);
        result.GetLength(1).Should().Be(4);
    }

    [Fact]
    public void GenerateMatrixPointerHashSet_IsCorrectLength()
    {
        // Act
        Dictionary<TimeOnly, int> result = AvailabilityMatrixTools.GenerateMatrixPointerHashSet(schedule);
        
        // Assert
        result.Count.Should().Be(4);
    }

}