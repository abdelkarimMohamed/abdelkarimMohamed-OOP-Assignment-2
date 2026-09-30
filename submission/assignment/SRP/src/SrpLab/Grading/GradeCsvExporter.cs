namespace SrpLab.Grading;

/// <summary>Responsibility: the CSV export layout of the grade book.</summary>
public sealed class GradeCsvExporter
{
    private readonly LetterGradePolicy _letters;
    private readonly HonorRollPolicy _honor;

    public GradeCsvExporter(LetterGradePolicy letters, HonorRollPolicy honor)
    {
        _letters = letters;
        _honor = honor;
    }

    public string Export(GradeBook book)
    {
        var rows = new List<string> { "studentId,average,letter,honor" };
        foreach (var id in book.StudentIds)
        {
            var avg = book.Average(id);
            rows.Add($"{id},{avg},{_letters.LetterFor(avg)},{(_honor.Qualifies(avg) ? 1 : 0)}");
        }
        return string.Join('\n', rows);
    }
}
