using LibrarySystem.Items;

namespace LibrarySystem.People;

/// <summary>Sets late-fee prices and withdraws/restores items. Gets a fixed 400 allowance.</summary>
public class HeadLibrarian : Staff
{
    private const decimal ResponsibilityAllowance = 400m;

    public HeadLibrarian(string personId, string fullName, string phone, DateTime hireDate, decimal monthlySalary)
        : base(personId, fullName, phone, hireDate, monthlySalary, ResponsibilityAllowance)
    {
    }

    public void ChangeLateFee(LibraryItem item, decimal newBaseLateFee)
    {
        item.ChangeBaseLateFee(newBaseLateFee);
    }

    public void WithdrawItem(LibraryItem item)
    {
        item.Withdraw();
    }

    public void RestoreItem(LibraryItem item)
    {
        item.Restore();
    }
}
