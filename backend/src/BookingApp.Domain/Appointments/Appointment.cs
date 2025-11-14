namespace BookingApp.Domain.Appointments;

/// <summary>
/// Wizyta
/// </summary>
public sealed class Appointment
{
    private Appointment() { }
    public Guid Id { get; private set; }
    public Guid CustormeId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Guid ServiceId { get; private set; }

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public AppointmentStatus Status { get; private set; }

    private Appointment(Guid id, Guid custormeId, Guid employeeId, Guid serviceId, DateTime startTime, DateTime endTime, AppointmentStatus status)
    {
        Id = id;
        CustormeId = custormeId;
        EmployeeId = employeeId;
        ServiceId = serviceId;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
    }

    public static Appointment Create(
        Guid custormeId,
        Guid employeeId,
        Guid serviceId,
        DateTime startTime,
        DateTime endTime,
        DateTime now
    )
    {
        if (startTime < now) throw new InvalidOperationException("Start time must be in the future");
        if (endTime <= startTime) throw new InvalidOperationException("End time must be after start time");

        return new Appointment(
            Guid.NewGuid(),
            custormeId,
            employeeId,
            serviceId,
            startTime,
            endTime,
            AppointmentStatus.Requested
        );
    }

    /// <summary>
    /// Sprawdza, czy ta wizyta nachodzi na inną (dla tego samego pracownika).
    /// Zakładamy przedział [StartTime, EndTime).
    /// </summary>
    public bool Overlaps(Appointment other)
    {
        // [Astart, Aend) i [Bstart, Bend) nakładają się, gdy:
        // Astart < Bend && Bstart < Aend
        return StartTime < other.EndTime &&
               other.StartTime < EndTime;
    }
}