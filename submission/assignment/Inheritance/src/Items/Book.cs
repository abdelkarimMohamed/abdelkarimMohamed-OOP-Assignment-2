namespace LibrarySystem.Items;

/// <summary>21-day loan, charges the base fee as is.</summary>
public class Book : LibraryItem
{
    private const int BookLoanDays = 21;
    private const decimal BookFeeMultiplier = 1.0m;

    public Book(string catalogNumber, string title, decimal baseLateFee)
        : base(catalogNumber, title, baseLateFee, BookLoanDays, BookFeeMultiplier)
    {
    }
}
