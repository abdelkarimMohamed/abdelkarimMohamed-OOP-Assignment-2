namespace SrpLab.Billing;

/// <summary>Responsibility: the collections escalation ladder (how serious a notice is).</summary>
public sealed class DunningSeverityPolicy
{
    public string For(int failedPayments) => failedPayments switch
    {
        <= 1 => "friendly reminder",
        2 => "second notice",
        _ => "final notice before suspension"
    };
}
