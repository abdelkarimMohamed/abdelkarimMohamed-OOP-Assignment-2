namespace LibrarySystem.People;

/// <summary>Up to 3 items at once, no fee discount.</summary>
public class StudentMember : Member
{
    private const int StudentMaxLoans = 3;
    private const decimal StudentDiscountPercent = 0m;

    public StudentMember(string personId, string fullName, string phone)
        : base(personId, fullName, phone, StudentMaxLoans, StudentDiscountPercent)
    {
    }
}
