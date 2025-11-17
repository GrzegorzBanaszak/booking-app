using System;

namespace BookingApp.Domain.Scheduling;

public sealed class WorkingHours
{
    private WorkingHours() { }

    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan Start { get; private set; } // np. 09:00
    public TimeSpan End { get; private set; }   // np. 17:00


    private WorkingHours(Guid id, Guid employeeId, DayOfWeek dayOfWeek, TimeSpan start, TimeSpan end)
    {
        if (end <= start) throw new InvalidOperationException("End time must be after start time.");

        Id = id;
        EmployeeId = employeeId;
        DayOfWeek = dayOfWeek;
        Start = start;
        End = end;
    }

    public static WorkingHours Create(Guid employeeId, DayOfWeek dayOfWeek, TimeSpan start, TimeSpan end) => new WorkingHours(Guid.NewGuid(), employeeId, dayOfWeek, start, end);

    /// <summary>
    /// Sprawdza, czy podany przedział [startTime, endTime)
    /// mieści się w tych godzinach pracy (dzień tygodnia + czas).
    /// </summary>
    public bool Contains(DateTime startTime, DateTime endTime)
    {
        if (startTime.DayOfWeek != DayOfWeek || endTime.DayOfWeek != DayOfWeek) return false;

        var startTod = startTime.TimeOfDay;
        var endTod = endTime.TimeOfDay;

        return startTod >= Start && endTod <= End;
    }
}
