namespace SrpLab.Warehouse;

public sealed record PickLine(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand);

/// <summary>Responsibility: hold the lines an order needs picked.</summary>
public sealed class PickList
{
    private readonly List<PickLine> _lines = new();

    public IReadOnlyList<PickLine> Lines => _lines;

    public void AddNeed(string sku, string aisle, int bin, int qtyNeeded, int qtyOnHand) =>
        _lines.Add(new PickLine(sku, aisle, bin, qtyNeeded, qtyOnHand));
}
