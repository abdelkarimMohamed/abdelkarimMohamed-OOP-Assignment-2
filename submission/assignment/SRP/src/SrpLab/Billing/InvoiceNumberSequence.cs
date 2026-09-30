namespace SrpLab.Billing;

/// <summary>Responsibility: mint invoice numbers (sequence + fiscal prefix). The only place with that side effect.</summary>
public sealed class InvoiceNumberSequence
{
    private int _last;

    public InvoiceNumberSequence(int startAfter = 1000) => _last = startAfter;

    public string Next(DateOnly periodStart) => $"INV-{periodStart:yyyyMM}-{++_last:D5}";
}
