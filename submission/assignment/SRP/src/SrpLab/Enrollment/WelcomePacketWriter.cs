namespace SrpLab.Enrollment;

/// <summary>Responsibility: marketing content of the welcome packet (Markdown).</summary>
public sealed class WelcomePacketWriter
{
    public string Write(CourseOffering course, CourseRoster roster, string studentEmail, string studentName)
    {
        var status = roster.IsSeated(studentEmail) ? "confirmed seat" : $"waitlist #{roster.WaitlistPosition(studentEmail)}";
        return $"# Welcome to {course.CourseCode}\nHi {studentName},\nYour status: **{status}**.\n" +
               $"Bring a laptop. Discord onboarding link: https://example.invalid/{course.CourseCode.ToLowerInvariant()}\n";
    }
}
