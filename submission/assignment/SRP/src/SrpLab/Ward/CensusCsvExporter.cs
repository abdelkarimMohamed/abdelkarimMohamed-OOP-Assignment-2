namespace SrpLab.Ward;

/// <summary>Responsibility: export the ward census in CSV shape.</summary>
public sealed class CensusCsvExporter
{
    public string Export(WardBoard board)
    {
        var lines = new List<string> { "bed,patient,acuity" };
        foreach (var a in board.AllByBedNumber())
            lines.Add($"{a.Bed},{a.PatientId},{a.Acuity}");
        return string.Join('\n', lines);
    }
}
