namespace SrpLab.Grading;

/// <summary>Responsibility: the registrar's plain-text transcript format.</summary>
public sealed class TranscriptWriter
{
    private readonly LetterGradePolicy _letters;
    private readonly HonorRollPolicy _honor;

    public TranscriptWriter(LetterGradePolicy letters, HonorRollPolicy honor)
    {
        _letters = letters;
        _honor = honor;
    }

    public string Write(GradeBook book, string studentId, string fullName)
    {
        var avg = book.Average(studentId);
        return $"TRANSCRIPT\nStudent: {fullName} ({studentId})\nAverage: {avg}\nLetter: {_letters.LetterFor(avg)}\nHonor: {_honor.Qualifies(avg)}\n";
    }
}
