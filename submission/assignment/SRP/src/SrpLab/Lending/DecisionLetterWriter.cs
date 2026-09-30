namespace SrpLab.Lending;

/// <summary>Responsibility: legal/communications wording of the decision letter.</summary>
public sealed class DecisionLetterWriter
{
    public string Write(string applicantName, decimal amount, decimal risk, bool isEligible, IReadOnlyList<string> documents)
    {
        if (isEligible)
        {
            return $"Dear {applicantName},\nYour request for {amount:C} is pre-approved (risk {risk:0}).\n" +
                   $"Please upload: {string.Join("; ", documents)}.\n";
        }

        return $"Dear {applicantName},\nWe are unable to approve {amount:C} at this time.\n" +
               $"Reference risk={risk:0}. You may reapply after improving documentation.\n";
    }
}
