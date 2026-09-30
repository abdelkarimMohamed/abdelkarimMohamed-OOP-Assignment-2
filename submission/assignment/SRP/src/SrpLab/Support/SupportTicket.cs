namespace SrpLab.Support;

/// <summary>Responsibility: hold the ticket's conversation data and its current priority.</summary>
public sealed class SupportTicket
{
    public string Id { get; }
    public string Subject { get; }
    public string Body { get; private set; }
    public DateTimeOffset OpenedAt { get; }
    public string Priority { get; private set; } = "P3";

    public SupportTicket(string id, string subject, string body, DateTimeOffset openedAt)
    {
        Id = id;
        Subject = subject;
        Body = body;
        OpenedAt = openedAt;
    }

    public void AppendCustomerMessage(string text) => Body += "\n---\n" + text;

    public void ChangePriority(string priority) => Priority = priority;
}
