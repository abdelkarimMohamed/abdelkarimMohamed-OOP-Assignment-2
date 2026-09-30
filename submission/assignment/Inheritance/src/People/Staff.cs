namespace LibrarySystem.People;

/// <summary>
/// Shared data and pay rule for every employee. The allowance differs per kind
/// and is passed by each child through : base(...), so one pay method works for all.
/// </summary>
public class Staff : Person
{
    private readonly decimal _responsibilityAllowance;

    public DateTime HireDate { get; }
    public decimal MonthlySalary { get; private set; }

    protected Staff(string personId, string fullName, string phone, DateTime hireDate, decimal monthlySalary, decimal responsibilityAllowance)
        : base(personId, fullName, phone)
    {
        if (monthlySalary <= 0)
            throw new ArgumentException("Monthly salary must be greater than zero.");
        if (responsibilityAllowance < 0)
            throw new ArgumentException("Responsibility allowance cannot be negative.");

        HireDate = hireDate;
        MonthlySalary = monthlySalary;
        _responsibilityAllowance = responsibilityAllowance;
    }

    public void GiveRaise(decimal percent)
    {
        if (percent <= 0)
            throw new ArgumentException($"A raise must be greater than zero percent (got {percent}).");

        MonthlySalary = MonthlySalary + (MonthlySalary * percent / 100m);
    }

    public decimal GetMonthlyPay()
    {
        return MonthlySalary + _responsibilityAllowance;
    }
}
