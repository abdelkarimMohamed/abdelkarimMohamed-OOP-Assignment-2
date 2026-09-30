namespace SrpLab.Checkout;

public sealed record BasketLine(string Sku, decimal Price, int Qty);

/// <summary>Responsibility: hold the basket lines and compute their raw subtotal.</summary>
public sealed class CheckoutBasket
{
    private readonly List<BasketLine> _lines = new();

    public IReadOnlyList<BasketLine> Lines => _lines;

    public void AddLine(string sku, decimal price, int qty)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
        _lines.Add(new BasketLine(sku, price, qty));
    }

    public decimal SubTotal() => _lines.Sum(l => l.Price * l.Qty);
}
