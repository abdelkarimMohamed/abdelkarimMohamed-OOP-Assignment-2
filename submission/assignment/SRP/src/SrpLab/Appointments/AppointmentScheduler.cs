namespace SrpLab.Appointments;

/// <summary>Responsibility: scheduling decisions — find the next free slot and book it if allowed.</summary>
public sealed class AppointmentScheduler
{
    private readonly BusinessHours _hours;
    private readonly AppointmentBook _book;

    public AppointmentScheduler(BusinessHours hours, AppointmentBook book)
    {
        _hours = hours;
        _book = book;
    }

    public DateTimeOffset? FindNextSlot(DateTimeOffset from, int searchHours)
    {
        var cursor = Align(from);
        var end = from.AddHours(searchHours);
        while (cursor < end)
        {
            if (_hours.IsWithin(cursor) && !_book.IsBooked(cursor)) return cursor;
            cursor = cursor.AddMinutes(_hours.SlotMinutes);
        }
        return null;
    }

    public bool TryBook(DateTimeOffset slot)
    {
        if (!_hours.IsWithin(slot) || _book.IsBooked(slot)) return false;
        _book.Add(slot);
        return true;
    }

    private DateTimeOffset Align(DateTimeOffset from)
    {
        var minutes = from.Minute - (from.Minute % _hours.SlotMinutes);
        return new DateTimeOffset(from.Year, from.Month, from.Day, from.Hour, minutes, 0, from.Offset);
    }
}
