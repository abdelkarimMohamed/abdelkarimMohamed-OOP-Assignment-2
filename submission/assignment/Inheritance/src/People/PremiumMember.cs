using LibrarySystem.Loans;

namespace LibrarySystem.People;

/// <summary>Up to 10 items at once, a fixed late-fee discount, and reading points.</summary>
public class PremiumMember : Member
{
    private const int PremiumMaxLoans = 10;
    private const int PointsPerReturnedLoan = 5;

    public PremiumMember(string personId, string fullName, string phone, decimal lateFeeDiscountPercent)
        : base(personId, fullName, phone, PremiumMaxLoans, lateFeeDiscountPercent)
    {
    }

    /// <summary>Computed from the history — never stored, never typed in.</summary>
    public int ReadingPoints
    {
        get
        {
            int returned = 0;
            foreach (Loan loan in Loans)
            {
                if (loan.Status == LoanStatus.Returned)
                    returned++;
            }
            return returned * PointsPerReturnedLoan;
        }
    }
}
