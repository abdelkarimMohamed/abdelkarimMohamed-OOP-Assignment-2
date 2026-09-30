namespace SrpLab.Checkout;

/// <summary>Responsibility: write the customer-facing gift card copy.</summary>
public sealed class GiftMessageCardWriter
{
    public string Write(CheckoutBasket basket, decimal total, string fromName)
    {
        var items = string.Join(", ", basket.Lines.Select(l => l.Sku));
        return $"Dear friend,\nA gift from {fromName} awaits ({items}).\nTotal surprise value: {total:C}\n";
    }
}
