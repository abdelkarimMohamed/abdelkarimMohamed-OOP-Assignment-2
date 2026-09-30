namespace SrpLab.Billing;

/// <summary>
/// Responsibility: the wording of the dunning email. It receives the invoice number
/// instead of minting one, so composing a message has no hidden side effect.
/// </summary>
public sealed class DunningEmailWriter
{
    private readonly DunningSeverityPolicy _severity;

    public DunningEmailWriter(DunningSeverityPolicy severity) => _severity = severity;

    public string Write(Subscription sub, string customerName, decimal balance, string invoiceNumber, DateOnly asOf) =>
        $"Subject: {_severity.For(sub.FailedPayments)} {invoiceNumber}\nHi {customerName},\n" +
        $"Balance {balance:C} as of {asOf:o} ({sub.FailedPayments} failures).\n";
}
