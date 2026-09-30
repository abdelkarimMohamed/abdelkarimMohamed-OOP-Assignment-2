namespace SrpLab.Support;

/// <summary>Responsibility: the operational SLA — response windows per priority and breach checks.</summary>
public sealed class SlaPolicy
{
    public DateTimeOffset DeadlineFor(SupportTicket ticket)
    {
        var hours = ticket.Priority switch
        {
            "P1" => 4,
            "P2" => 24,
            _ => 72
        };
        return ticket.OpenedAt.AddHours(hours);
    }

    public bool IsBreached(SupportTicket ticket, DateTimeOffset now) => now > DeadlineFor(ticket);
}
