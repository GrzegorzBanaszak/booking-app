using System;
using BookingApp.Domain.Scheduling;
using FluentAssertions;

namespace BookingApp.Domain.Tests.Scheduling;

public class WorkingHoursTests
{
    [Fact]
    public void Contains_ShouldReturnTrue_WhenAppointmentIsWithinWorkingHours()
    {
        // Arrange
        var employeeId = Guid.NewGuid();

        var workingHours = WorkingHours.Create(
            employeeId,
            DayOfWeek.Monday,
            start: new TimeSpan(8, 0, 0),
            end: new TimeSpan(17, 0, 0)
        );

        var day = new DateTime(2025, 1, 6);
        var startTime = day.AddHours(10);
        var endTime = day.AddHours(11);

        //Act
        var result = workingHours.Contains(startTime, endTime);

        //Assert
        result.Should().BeTrue();

    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenAppointmentStartsBeforeWorkingHours()
    {
        // Arrange
        var employeeId = Guid.NewGuid();

        var workingHours = WorkingHours.Create(
            employeeId,
            DayOfWeek.Monday,
            start: new TimeSpan(9, 0, 0),
            end: new TimeSpan(17, 0, 0));

        var day = new DateTime(2025, 1, 6);
        var startTime = day.AddHours(8);   // 08:00
        var endTime = day.AddHours(10);  // 10:00

        // Act
        var result = workingHours.Contains(startTime, endTime);

        // Assert
        result.Should().BeFalse();
    }
}
