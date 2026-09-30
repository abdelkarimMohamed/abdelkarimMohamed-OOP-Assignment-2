namespace SrpLab.Appointments;

/// <summary>Responsibility: remember which slots are already taken.</summary>
public sealed class AppointmentBook
{
    private readonly HashSet<DateTimeOffset> _booked = new();

    public bool IsBooked(DateTimeOffset slot) => _booked.Contains(slot);

    public void Add(DateTimeOffset slot) => _booked.Add(slot);
}
