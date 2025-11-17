namespace BookingApp.Domain.Appointments;

/// <summary>
/// Wizyta
/// </summary>
public sealed class Appointment
{
    private Appointment() { }

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Guid ServiceId { get; private set; }

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public AppointmentStatus Status { get; private set; }

    private Appointment(
        Guid id,
        Guid customerId,
        Guid employeeId,
        Guid serviceId,
        DateTime startTime,
        DateTime endTime,
        AppointmentStatus status)
    {
        Id = id;
        CustomerId = customerId;
        EmployeeId = employeeId;
        ServiceId = serviceId;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
    }

    /// <summary>
    /// Tworzy wizytę – na razie z jedną regułą:
    /// - startTime nie może być w przeszłości (względem now).
    /// </summary>
    public static Appointment Create(
        Guid customerId,
        Guid employeeId,
        Guid serviceId,
        DateTime startTime,
        DateTime endTime,
        DateTime now)
    {
        if (startTime < now)
        {
            throw new InvalidOperationException("Appointment cannot start in the past.");
        }

        if (endTime <= startTime)
        {
            throw new InvalidOperationException("End time must be after start time.");
        }

        return new Appointment(
            Guid.NewGuid(),
            customerId,
            employeeId,
            serviceId,
            startTime,
            endTime,
            AppointmentStatus.Requested);
    }

    public bool Overlaps(Appointment other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }
        return StartTime < other.EndTime && EndTime > other.StartTime;
    }
}