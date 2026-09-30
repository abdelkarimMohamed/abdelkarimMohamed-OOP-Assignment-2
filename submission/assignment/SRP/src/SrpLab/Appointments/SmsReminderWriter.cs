namespace SrpLab.Appointments;

/// <summary>Responsibility: the SMS reminder wording.</summary>
public sealed class SmsReminderWriter
{
    public string Write(DateTimeOffset slot, string clinicPhone) =>
        $"Reminder: appointment {slot:MMM dd HH:mm}. Call {clinicPhone} to reschedule.";
}
