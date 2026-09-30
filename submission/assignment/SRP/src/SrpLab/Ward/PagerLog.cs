namespace SrpLab.Ward;

/// <summary>Responsibility: record pager codes that were fired and hand them over once.</summary>
public sealed class PagerLog
{
    private readonly List<string> _entries = new();

    public void FireCodeYellow(int bed) => _entries.Add($"CODE-YELLOW bed={bed} at {DateTime.UtcNow:HH:mm}");

    public IReadOnlyList<string> Drain()
    {
        var copy = _entries.ToList();
        _entries.Clear();
        return copy;
    }
}
