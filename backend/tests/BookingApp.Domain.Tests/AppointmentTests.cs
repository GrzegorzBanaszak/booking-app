using BookingApp.Domain.Appointments;
using Xunit;

namespace BookingApp.Domain.Tests.Appointments;

public class AppointmentTests
{
    [Fact]
    public void Overlaps_ShouldReturnTrue_WhenAppointmentsOverlap()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var a = Appointment.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            now.AddHours(10),
            now.AddHours(11),
            now);

        var b = Appointment.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            now.AddHours(10).AddMinutes(30),
            now.AddHours(11).AddMinutes(30),
            now);

        // Act
        var result = a.Overlaps(b);

        // Assert
        Assert.True(result);
    }
}
