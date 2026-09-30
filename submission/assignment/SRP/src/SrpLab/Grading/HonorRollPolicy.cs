namespace SrpLab.Grading;

/// <summary>Responsibility: the honor-roll qualification rule.</summary>
public sealed class HonorRollPolicy
{
    private readonly LetterGradePolicy _letters;

    public HonorRollPolicy(LetterGradePolicy letters) => _letters = letters;

    public bool Qualifies(decimal average) => average >= 85 && _letters.LetterFor(average) is "A" or "B";
}
