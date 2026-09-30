namespace SrpLab.Lending;

/// <summary>Responsibility: decide approve / not approve from the risk score and minimum credit.</summary>
public sealed class EligibilityRule
{
    private readonly RiskScorer _risk;

    public EligibilityRule(RiskScorer risk) => _risk = risk;

    public bool IsEligible(LoanApplication app) => _risk.Score(app) >= 55m && app.CreditScore >= 580;
}
