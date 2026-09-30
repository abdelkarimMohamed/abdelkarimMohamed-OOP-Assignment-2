namespace SrpLab.Appointments;

/// <summary>Responsibility: serialize an appointment to the ICS calendar format.</summary>
public sealed class IcsCalendarWriter
{
    public string Write(DateTimeOffset slot, int durationMinutes, string patientName, string clinician)
    {
        var uid = Guid.NewGuid();
        var end = slot.AddMinutes(durationMinutes);
        return "BEGIN:VCALENDAR\nVERSION:2.0\nBEGIN:VEVENT\n" +
               $"UID:{uid}\nDTSTART:{slot:yyyyMMdd'T'HHmmss'Z'}\nDTEND:{end:yyyyMMdd'T'HHmmss'Z'}\n" +
               $"SUMMARY:Visit {patientName} / {clinician}\nEND:VEVENT\nEND:VCALENDAR\n";
    }
}
