using LibrarySystem.Loans;

namespace LibrarySystem.People;

/// <summary>Processes returns and marks items as lost.</summary>
public class Librarian : Staff
{
    private const decimal NoAllowance = 0m;

    public Librarian(string personId, string fullName, string phone, DateTime hireDate, decimal monthlySalary)
        : base(personId, fullName, phone, hireDate, monthlySalary, NoAllowance)
    {
    }

    public void ProcessReturn(Loan loan, DateTime returnDate)
    {
        loan.MarkReturned(returnDate);
    }

    public void MarkAsLost(Loan loan)
    {
        loan.MarkLost();
    }
}
