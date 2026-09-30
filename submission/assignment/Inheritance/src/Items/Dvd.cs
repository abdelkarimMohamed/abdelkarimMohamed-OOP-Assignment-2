namespace LibrarySystem.Items;

/// <summary>7-day loan, charges double the base fee.</summary>
public class Dvd : LibraryItem
{
    private const int DvdLoanDays = 7;
    private const decimal DvdFeeMultiplier = 2.0m;

    public Dvd(string catalogNumber, string title, decimal baseLateFee)
        : base(catalogNumber, title, baseLateFee, DvdLoanDays, DvdFeeMultiplier)
    {
    }
}
