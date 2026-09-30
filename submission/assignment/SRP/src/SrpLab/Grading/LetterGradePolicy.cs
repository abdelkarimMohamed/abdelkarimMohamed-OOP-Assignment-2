namespace SrpLab.Grading;

/// <summary>Responsibility: the faculty's letter-band policy.</summary>
public sealed class LetterGradePolicy
{
    public string LetterFor(decimal average)
    {
        if (average >= 90) return "A";
        if (average >= 80) return "B";
        if (average >= 70) return "C";
        if (average >= 60) return "D";
        return "F";
    }
}
