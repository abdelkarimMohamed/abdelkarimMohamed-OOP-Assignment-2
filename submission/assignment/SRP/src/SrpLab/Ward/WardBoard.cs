namespace SrpLab.Ward;

/// <summary>Responsibility: keep track of which patient occupies which bed (the ward census).</summary>
public sealed class WardBoard
{
    private readonly Dictionary<int, BedAssignment> _beds = new();

    public BedAssignment Assign(int bed, string patientId, int acuity)
    {
        if (bed <= 0) throw new ArgumentOutOfRangeException(nameof(bed));
        if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentException("patient required");

        var assignment = new BedAssignment(bed, patientId.Trim().ToUpperInvariant(), acuity);
        _beds[bed] = assignment;
        return assignment;
    }

    public BedAssignment? Find(int bed) => _beds.TryGetValue(bed, out var a) ? a : null;

    public IReadOnlyList<BedAssignment> AllByBedNumber() => _beds.Values.OrderBy(b => b.Bed).ToList();
}
