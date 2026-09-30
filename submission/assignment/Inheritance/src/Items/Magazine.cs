namespace LibrarySystem.Items;

/// <summary>3-day loan, charges half the base fee.</summary>
public class Magazine : LibraryItem
{
    private const int MagazineLoanDays = 3;
    private const decimal MagazineFeeMultiplier = 0.5m;

    public Magazine(string catalogNumber, string title, decimal baseLateFee)
        : base(catalogNumber, title, baseLateFee, MagazineLoanDays, MagazineFeeMultiplier)
    {
    }
}
