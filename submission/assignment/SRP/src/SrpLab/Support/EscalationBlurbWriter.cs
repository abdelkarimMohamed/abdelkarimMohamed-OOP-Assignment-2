namespace SrpLab.Support;

/// <summary>Responsibility: format the internal escalation message for the on-call team.</summary>
public sealed class EscalationBlurbWriter
{
    public string Write(SupportTicket ticket, DateTimeOffset breachAt) =>
        $"ESCALATE {ticket.Id} priority={ticket.Priority} breachAt={breachAt:u} keywords-scanned=yes";
}
