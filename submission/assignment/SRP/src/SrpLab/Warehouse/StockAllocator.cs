namespace SrpLab.Warehouse;

public sealed record Allocation(PickLine Line, int Allocated)
{
    public bool IsShort => Allocated < Line.QtyNeeded;
}

/// <summary>Responsibility: the allocation/backorder policy — how much of each line can be filled.</summary>
public sealed class StockAllocator
{
    public IReadOnlyList<Allocation> Allocate(PickList list) =>
        list.Lines.Select(l => new Allocation(l, Math.Min(l.QtyNeeded, l.QtyOnHand))).ToList();
}
