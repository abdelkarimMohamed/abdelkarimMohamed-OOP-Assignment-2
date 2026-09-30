namespace SrpLab.Warehouse;

public sealed record PickStop(string Aisle, int Bin, string Sku, int Qty);

/// <summary>Responsibility: the walking-path heuristic through the warehouse.</summary>
public sealed class PickRoutePlanner
{
    public IReadOnlyList<PickStop> Plan(IReadOnlyList<Allocation> allocations) =>
        allocations
            .Where(a => a.Allocated > 0)
            .OrderBy(a => a.Line.Aisle)
            .ThenBy(a => a.Line.Bin)
            .Select(a => new PickStop(a.Line.Aisle, a.Line.Bin, a.Line.Sku, a.Allocated))
            .ToList();
}
