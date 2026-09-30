namespace PatternsLab.Problems.Builder;

public enum AccessMode
{
    LiveGroup,
    VideosOnly
}

/// <summary>
/// Builder: the only way to create a registration is
/// CourseRegistration.For(email, course, mode)  ← required values (R2)
///     .InGroup(..).WithDiscount(..)...          ← optional, fluent steps (R1)
///     .Build();                                 ← validates the rules (R3)
/// The constructor is private, so an unvalidated registration can never exist.
/// </summary>
public sealed class CourseRegistration
{
    public string StudentEmail { get; }
    public string CourseCode { get; }
    public AccessMode AccessMode { get; }
    public string? GroupCode { get; }
    public string? DiscountCode { get; }
    public bool SendWhatsApp { get; }
    public bool SendEmailWelcome { get; }
    public string? MentorNote { get; }
    public DateOnly? PreferredStart { get; }

    private CourseRegistration(Builder b)
    {
        StudentEmail = b.StudentEmail;
        CourseCode = b.CourseCode;
        AccessMode = b.AccessMode;
        GroupCode = b.GroupCode;
        DiscountCode = b.DiscountCode;
        SendWhatsApp = b.SendWhatsApp;
        SendEmailWelcome = b.SendEmailWelcome;
        MentorNote = b.MentorNote;
        PreferredStart = b.PreferredStart;
    }

    /// <summary>Starts a builder. The three required values are the parameters.</summary>
    public static Builder For(string studentEmail, string courseCode, AccessMode accessMode) =>
        new(studentEmail, courseCode, accessMode);

    public override string ToString()
        => $"{StudentEmail} → {CourseCode} [{AccessMode}] group={GroupCode ?? "-"} discount={DiscountCode ?? "-"} wa={SendWhatsApp} mail={SendEmailWelcome}";

    public sealed class Builder
    {
        internal string StudentEmail { get; }
        internal string CourseCode { get; }
        internal AccessMode AccessMode { get; }
        internal string? GroupCode { get; private set; }
        internal string? DiscountCode { get; private set; }
        internal bool SendWhatsApp { get; private set; }
        internal bool SendEmailWelcome { get; private set; }
        internal string? MentorNote { get; private set; }
        internal DateOnly? PreferredStart { get; private set; }

        internal Builder(string studentEmail, string courseCode, AccessMode accessMode)
        {
            StudentEmail = studentEmail;
            CourseCode = courseCode;
            AccessMode = accessMode;
        }

        public Builder InGroup(string groupCode) { GroupCode = groupCode; return this; }
        public Builder WithDiscount(string discountCode) { DiscountCode = discountCode; return this; }
        public Builder NotifyByWhatsApp() { SendWhatsApp = true; return this; }
        public Builder SendWelcomeEmail() { SendEmailWelcome = true; return this; }
        public Builder WithMentorNote(string note) { MentorNote = note; return this; }
        public Builder PreferringStart(DateOnly date) { PreferredStart = date; return this; }

        public CourseRegistration Build()
        {
            if (string.IsNullOrWhiteSpace(StudentEmail))
                throw new ArgumentException("Student email is required.");
            if (string.IsNullOrWhiteSpace(CourseCode))
                throw new ArgumentException("Course code is required.");

            if (AccessMode == AccessMode.LiveGroup && string.IsNullOrWhiteSpace(GroupCode))
                throw new InvalidOperationException(
                    $"A LiveGroup registration for {StudentEmail} needs a group code — call InGroup(...) before Build().");
            if (AccessMode == AccessMode.VideosOnly && !string.IsNullOrWhiteSpace(GroupCode))
                throw new InvalidOperationException(
                    $"A VideosOnly registration for {StudentEmail} cannot have a group code (got '{GroupCode}').");

            return new CourseRegistration(this);
        }
    }
}

public static class RegistrationCallSites
{
    public static CourseRegistration CreateLiveStudent() =>
        CourseRegistration.For("sara@mail.com", "SEF-101", AccessMode.LiveGroup)
            .InGroup("G1")
            .WithDiscount("EARLY10")
            .NotifyByWhatsApp()
            .SendWelcomeEmail()
            .WithMentorNote("Needs evening slot")
            .PreferringStart(new DateOnly(2026, 10, 1))
            .Build();

    public static CourseRegistration CreateVideosOnly() =>
        CourseRegistration.For("ali@mail.com", "SEF-101", AccessMode.VideosOnly)
            .SendWelcomeEmail()
            .Build();
}
