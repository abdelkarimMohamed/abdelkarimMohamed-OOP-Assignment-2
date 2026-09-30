using LibrarySystem.Items;
using LibrarySystem.Loans;

namespace LibrarySystem.People;

/// <summary>
/// A borrower. Owns its loan history and enforces the loan limit.
/// Children pass their own limit and discount through : base(...).
/// </summary>
public class Member : Person
{
    private readonly List<Loan> _loans = new List<Loan>();

    public int MaxActiveLoans { get; }
    public decimal LateFeeDiscountPercent { get; }

    /// <summary>Read-only view: outside code can read the history but cannot Add or Remove.</summary>
    public IReadOnlyList<Loan> Loans
    {
        get { return _loans.AsReadOnly(); }
    }

    public int ActiveLoanCount
    {
        get
        {
            int count = 0;
            foreach (Loan loan in _loans)
            {
                if (loan.Status == LoanStatus.Borrowed)
                    count++;
            }
            return count;
        }
    }

    protected Member(string personId, string fullName, string phone, int maxActiveLoans, decimal lateFeeDiscountPercent)
        : base(personId, fullName, phone)
    {
        if (maxActiveLoans <= 0)
            throw new ArgumentException("Max active loans must be greater than zero.");
        if (lateFeeDiscountPercent < 0 || lateFeeDiscountPercent > 100)
            throw new ArgumentException("Late-fee discount must be between 0 and 100 percent.");

        MaxActiveLoans = maxActiveLoans;
        LateFeeDiscountPercent = lateFeeDiscountPercent;
    }

    /// <summary>The only way a loan is created and added to the history.</summary>
    public Loan Borrow(LibraryItem item, DateTime borrowDate)
    {
        if (ActiveLoanCount >= MaxActiveLoans)
            throw new InvalidOperationException(
                $"{FullName} already has {ActiveLoanCount} active loans (limit is {MaxActiveLoans}).");

        // The Loan constructor checks the item (withdrawn / already on loan) before anything is stored.
        Loan loan = new Loan(this, item, borrowDate);
        _loans.Add(loan);
        return loan;
    }
}
