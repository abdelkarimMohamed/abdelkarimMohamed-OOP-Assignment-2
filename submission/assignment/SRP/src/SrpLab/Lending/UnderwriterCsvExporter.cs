namespace SrpLab.Lending;

/// <summary>Responsibility: the analytics export row schema for underwriters.</summary>
public sealed class UnderwriterCsvExporter
{
    public string Row(string applicationId, LoanApplication app, decimal risk, bool isEligible) =>
        $"{applicationId},{app.CreditScore},{app.EmploymentMonths},{(app.HasCollateral ? 1 : 0)},{risk:0.00},{(isEligible ? "Y" : "N")}";
}
