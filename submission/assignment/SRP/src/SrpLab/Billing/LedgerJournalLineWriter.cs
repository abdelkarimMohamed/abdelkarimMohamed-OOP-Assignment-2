namespace SrpLab.Billing;

/// <summary>Responsibility: the accounting export line format.</summary>
public sealed class LedgerJournalLineWriter
{
    public string Write(Subscription sub, string invoiceNumber, decimal amount) =>
        $"{sub.CustomerId},{invoiceNumber},{amount:0.00},AR-SUB";
}
