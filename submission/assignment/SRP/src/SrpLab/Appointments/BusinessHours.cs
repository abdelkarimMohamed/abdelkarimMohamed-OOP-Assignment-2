namespace SrpLab.Appointments;

/// <summary>Responsibility: the clinic's opening-hours policy (open days, open/close time, slot length).</summary>
public sealed class BusinessHours
{
    public TimeOnly Open { get; }
    public TimeOnly Close { get; }
    public int SlotMinutes { get; }

    public BusinessHours(TimeOnly open, TimeOnly close, int slotMinutes)
    {
        Open = open;
        Close = close;
        SlotMinutes = slotMinutes;
    }

    public bool IsWithin(DateTimeOffset when)
    {
        if (when.DayOfWeek is DayOfWeek.Friday or DayOfWeek.Saturday) return false;
        var t = TimeOnly.FromDateTime(when.DateTime);
        return t >= Open && t.AddMinutes(SlotMinutes) <= Close;
    }
}
