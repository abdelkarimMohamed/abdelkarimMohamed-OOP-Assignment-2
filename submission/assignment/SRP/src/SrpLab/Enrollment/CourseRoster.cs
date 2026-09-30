namespace SrpLab.Enrollment;

/// <summary>Responsibility: seat allocation — who is seated, who waits, and promotion from the waitlist.</summary>
public sealed class CourseRoster
{
    private readonly HashSet<string> _seated = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _waitlist = new();
    private readonly int _capacity;

    public CourseRoster(CourseOffering offering) => _capacity = offering.Capacity;

    public string Register(string studentEmail)
    {
        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email");
        var email = studentEmail.Trim();

        if (_seated.Contains(email) || _waitlist.Contains(email))
            return "ALREADY_REGISTERED";

        if (_seated.Count < _capacity)
        {
            _seated.Add(email);
            return "SEATED";
        }

        _waitlist.Add(email);
        return $"WAITLIST:{_waitlist.Count}";
    }

    public bool IsSeated(string studentEmail) => _seated.Contains(studentEmail);

    public int WaitlistPosition(string studentEmail)
    {
        var idx = _waitlist.FindIndex(x => x.Equals(studentEmail, StringComparison.OrdinalIgnoreCase));
        return idx < 0 ? -1 : idx + 1;
    }

    public void PromoteFromWaitlist(int seats)
    {
        while (seats > 0 && _waitlist.Count > 0 && _seated.Count < _capacity)
        {
            var next = _waitlist[0];
            _waitlist.RemoveAt(0);
            _seated.Add(next);
            seats--;
        }
    }
}
