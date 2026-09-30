namespace LibrarySystem.People;

/// <summary>Assigned to one section; the section changes only through Reassign.</summary>
public class Shelver : Staff
{
    private const decimal NoAllowance = 0m;

    public string Section { get; private set; }

    public Shelver(string personId, string fullName, string phone, DateTime hireDate, decimal monthlySalary, string section)
        : base(personId, fullName, phone, hireDate, monthlySalary, NoAllowance)
    {
        if (string.IsNullOrWhiteSpace(section))
            throw new ArgumentException("Section must not be empty.");

        Section = section;
    }

    public void Reassign(string newSection)
    {
        if (string.IsNullOrWhiteSpace(newSection))
            throw new ArgumentException("New section must not be empty.");

        Section = newSection;
    }
}
