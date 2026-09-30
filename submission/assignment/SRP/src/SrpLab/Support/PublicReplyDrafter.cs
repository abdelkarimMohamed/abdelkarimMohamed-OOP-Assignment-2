namespace SrpLab.Support;

/// <summary>Responsibility: CX wording of the public reply to the customer.</summary>
public sealed class PublicReplyDrafter
{
    public string Draft(SupportTicket ticket, DateTimeOffset nextUpdateBefore, string agentName)
    {
        var apology = ticket.Priority == "P1" ? "We are treating this as a critical incident." : "Thanks for reaching out.";
        return $"Hi,\n{apology}\nTicket {ticket.Id} is with {agentName}. Next update before {nextUpdateBefore:u}.\n";
    }
}
