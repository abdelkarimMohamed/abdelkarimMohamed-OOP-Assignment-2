namespace SrpLab.Warehouse;

/// <summary>Responsibility: the handheld-device wording shown to the picker.</summary>
public sealed class PickerScriptWriter
{
    public string Write(IReadOnlyList<PickStop> route, IReadOnlyList<Allocation> allocations)
    {
        var steps = route.Select((s, i) => $"{i + 1}. Go aisle {s.Aisle} bin {s.Bin}: pick {s.Qty} × {s.Sku}");
        var shortfalls = allocations.Where(a => a.IsShort).Select(a => a.Line.Sku).ToList();
        var warn = shortfalls.Count > 0 ? "SHORTAGES: " + string.Join(", ", shortfalls) : "SHORTAGES: none";
        return string.Join('\n', steps) + "\n" + warn;
    }
}
