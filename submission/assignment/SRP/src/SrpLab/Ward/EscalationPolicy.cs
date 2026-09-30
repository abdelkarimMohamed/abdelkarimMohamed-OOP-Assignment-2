namespace SrpLab.Ward;

/// <summary>Responsibility: decide the escalation thresholds (when to page, what status label applies).</summary>
public sealed class EscalationPolicy
{
    private const int EscalateThreshold = 8;
    private const int WatchThreshold = 4;

    public bool RequiresPage(int acuity) => acuity >= EscalateThreshold;

    public string StatusFor(int acuity) =>
        acuity >= EscalateThreshold ? "ESCALATE" : acuity >= WatchThreshold ? "WATCH" : "STABLE";
}
