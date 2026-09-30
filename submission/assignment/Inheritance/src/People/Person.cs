namespace LibrarySystem.People;

/// <summary>
/// Shared identity for everyone in the system. The constructor is protected,
/// so a plain Person can never be created — only a member or a staff member.
/// </summary>
public class Person
{
    public string PersonId { get; }
    public string FullName { get; }
    public string Phone { get; }

    protected Person(string personId, string fullName, string phone)
    {
        if (string.IsNullOrWhiteSpace(personId))
            throw new ArgumentException("Person ID must not be empty.");
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name must not be empty.");
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone must not be empty.");

        PersonId = personId;
        FullName = fullName;
        Phone = phone;
    }
}
