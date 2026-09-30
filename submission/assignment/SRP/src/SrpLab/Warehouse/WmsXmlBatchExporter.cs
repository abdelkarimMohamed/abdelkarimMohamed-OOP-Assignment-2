namespace SrpLab.Warehouse;

/// <summary>Responsibility: the XML integration contract with the WMS.</summary>
public sealed class WmsXmlBatchExporter
{
    public string Export(string batchId, IReadOnlyList<Allocation> allocations)
    {
        var parts = allocations.Select(a => $"<line sku=\"{a.Line.Sku}\" qty=\"{a.Allocated}\" />");
        return $"<batch id=\"{batchId}\">{string.Join("", parts)}</batch>";
    }
}
