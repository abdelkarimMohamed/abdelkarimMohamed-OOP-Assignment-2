namespace LibrarySystem.Items;

/// <summary>
/// Shared data and rules for every lendable item. Loan period and late-fee multiplier
/// differ per kind and are passed by each child through : base(...), so DailyLateFee
/// works for every item without checking its type.
/// </summary>
public class LibraryItem
{
    private readonly decimal _lateFeeMultiplier;

    public string CatalogNumber { get; }
    public string Title { get; }
    public int LoanPeriodDays { get; }
    public decimal BaseLateFee { get; private set; }
    public bool IsWithdrawn { get; private set; }
    public bool IsOnLoan { get; private set; }

    /// <summary>What is actually charged per late day for this kind of item.</summary>
    public decimal DailyLateFee
    {
        get { return BaseLateFee * _lateFeeMultiplier; }
    }

    protected LibraryItem(string catalogNumber, string title, decimal baseLateFee, int loanPeriodDays, decimal lateFeeMultiplier)
    {
        if (string.IsNullOrWhiteSpace(catalogNumber))
            throw new ArgumentException("Catalog number must not be empty.");
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title must not be empty.");
        if (baseLateFee <= 0)
            throw new ArgumentException("Base late fee must be greater than zero.");
        if (loanPeriodDays <= 0)
            throw new ArgumentException("Loan period must be greater than zero.");
        if (lateFeeMultiplier <= 0)
            throw new ArgumentException("Late-fee multiplier must be greater than zero.");

        CatalogNumber = catalogNumber;
        Title = title;
        BaseLateFee = baseLateFee;
        LoanPeriodDays = loanPeriodDays;
        _lateFeeMultiplier = lateFeeMultiplier;
    }

    // ---- pricing (called by HeadLibrarian) ----
    internal void ChangeBaseLateFee(decimal newBaseLateFee)
    {
        if (newBaseLateFee <= 0)
            throw new ArgumentException($"A late fee must be greater than zero (got {newBaseLateFee}).");

        BaseLateFee = newBaseLateFee;
    }

    // ---- circulation (called by HeadLibrarian) ----
    internal void Withdraw()
    {
        if (IsWithdrawn)
            throw new InvalidOperationException($"'{Title}' is already withdrawn.");

        IsWithdrawn = true;
    }

    internal void Restore()
    {
        if (!IsWithdrawn)
            throw new InvalidOperationException($"'{Title}' is not withdrawn, so it cannot be restored.");

        IsWithdrawn = false;
    }

    // ---- side effects of borrowing / returning (called by Loan) ----
    internal void MarkAsBorrowed()
    {
        if (IsWithdrawn)
            throw new InvalidOperationException($"'{Title}' is withdrawn from circulation and cannot be borrowed.");
        if (IsOnLoan)
            throw new InvalidOperationException($"'{Title}' is already on loan to someone else.");

        IsOnLoan = true;
    }

    internal void MarkAsReturned()
    {
        IsOnLoan = false;
    }
}
