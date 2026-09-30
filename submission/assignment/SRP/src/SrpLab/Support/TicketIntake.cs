namespace SrpLab.Support;

/// <summary>Responsibility: open tickets and take in customer messages, keeping priority up to date.</summary>
public sealed class TicketIntake
{
    private readonly TicketPriorityClassifier _classifier;

    public TicketIntake(TicketPriorityClassifier classifier) => _classifier = classifier;

    public SupportTicket Open(string id, string subject, string body, DateTimeOffset openedAt)
    {
        var ticket = new SupportTicket(id, subject, body, openedAt);
        Reclassify(ticket);
        return ticket;
    }

    public void AddCustomerMessage(SupportTicket ticket, string text)
    {
        ticket.AppendCustomerMessage(text);
        Reclassify(ticket);
    }

    private void Reclassify(SupportTicket ticket) =>
        ticket.ChangePriority(_classifier.Classify(ticket.Subject, ticket.Body));
}
