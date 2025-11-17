using BookingApp.Domain.Appointments;
using FluentAssertions;
using Xunit;

namespace BookingApp.Domain.Tests.Appointments;

public class AppointmentTests
{
    [Fact]
    public void Create_ShouldThrow_WhenStartTimeIsInThePast()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var customerId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var startTime = now.AddMinutes(-30);
        var endTime = now.AddMinutes(30);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            Appointment.Create(
                customerId,
                employeeId,
                serviceId,
                startTime,
                endTime,
                now)
        );

        Assert.Equal("Appointment cannot start in the past.", exception.Message);
    }

    [Fact]
    public void Overlaps_ShouldReturnTrue_WhenAppointmentsOverlap()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var customerId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var a = Appointment.Create(
            customerId,
            employeeId,
            serviceId,
            startTime: now.AddHours(10),
            endTime: now.AddHours(11),
            now: now
        );

        var b = Appointment.Create(
            customerId,
            employeeId,
            serviceId,
            startTime: now.AddHours(10).AddMinutes(30),
            endTime: now.AddHours(11).AddMinutes(30),
            now: now
        );

        // Act
        var result = a.Overlaps(b);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Overlaps_ShouldReturnFalse_WhenAppointmentsOverlap()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var customerId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var a = Appointment.Create(
            customerId,
            employeeId,
            serviceId,
            startTime: now.AddHours(10),
            endTime: now.AddHours(11),
            now: now
        );

        var b = Appointment.Create(
            customerId,
            employeeId,
            serviceId,
            startTime: now.AddHours(11),
            endTime: now.AddHours(12),
            now: now
        );

        // Act
        var result = a.Overlaps(b);

        // Assert
        result.Should().BeFalse();
    }
}


