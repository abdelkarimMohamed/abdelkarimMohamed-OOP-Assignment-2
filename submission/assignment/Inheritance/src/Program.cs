using LibrarySystem.Items;
using LibrarySystem.Loans;
using LibrarySystem.People;

Console.WriteLine("=== City Library — proving the rules ===");

// ─────────────────────────────────────────────────────────────
// Setup
// ─────────────────────────────────────────────────────────────
StudentMember student = new StudentMember("M-100", "Ali Hassan", "01000000001");
PremiumMember premium = new PremiumMember("M-200", "Sara Adel", "01000000002", 20m);

Librarian librarian = new Librarian("S-1", "Omar Nabil", "01100000001", new DateTime(2022, 3, 1), 8000m);
HeadLibrarian head = new HeadLibrarian("S-2", "Mona Fathy", "01100000002", new DateTime(2018, 1, 15), 12000m);
Shelver shelver = new Shelver("S-3", "Karim Samy", "01100000003", new DateTime(2024, 6, 1), 6000m, "Fiction");

Book book = new Book("B-001", "Clean Code", 5m);
Dvd dvd = new Dvd("D-001", "Inception", 10m);
Magazine magazine = new Magazine("MG-001", "National Geographic", 4m);

DateTime day1 = new DateTime(2026, 9, 1);

// ─────────────────────────────────────────────────────────────
// 1. Borrowing rules — all must be rejected
// ─────────────────────────────────────────────────────────────
Console.WriteLine("\n--- Borrowing rules ---");

Book withdrawnBook = new Book("B-002", "Old Atlas", 5m);
head.WithdrawItem(withdrawnBook);
try
{
    student.Borrow(withdrawnBook, day1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected (withdrawn): " + ex.Message);
}

Book popularBook = new Book("B-003", "The Pragmatic Programmer", 5m);
premium.Borrow(popularBook, day1);
try
{
    student.Borrow(popularBook, day1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected (already on loan): " + ex.Message);
}

student.Borrow(new Book("B-010", "Book One", 5m), day1);
student.Borrow(new Book("B-011", "Book Two", 5m), day1);
student.Borrow(new Book("B-012", "Book Three", 5m), day1);
try
{
    student.Borrow(new Book("B-013", "Book Four", 5m), day1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected (loan limit): " + ex.Message);
}

// ─────────────────────────────────────────────────────────────
// 2. Staff: one List<Staff>, one pay method, no type checks
// ─────────────────────────────────────────────────────────────
Console.WriteLine("\n--- Staff monthly pay ---");

List<Staff> staff = new List<Staff>();
staff.Add(librarian);
staff.Add(head);
staff.Add(shelver);
foreach (Staff member in staff)
{
    Console.WriteLine($"{member.FullName,-12} monthly pay: {member.GetMonthlyPay():0.00}");
}

librarian.GiveRaise(10m);
Console.WriteLine($"After a 10% raise, {librarian.FullName} earns {librarian.GetMonthlyPay():0.00}");
try
{
    librarian.GiveRaise(0m);
}
catch (ArgumentException ex)
{
    Console.WriteLine("Rejected (raise): " + ex.Message);
}

shelver.Reassign("Children");
Console.WriteLine($"{shelver.FullName} now shelves: {shelver.Section}");

// ─────────────────────────────────────────────────────────────
// 3. Items: one List<LibraryItem>, one daily-fee property, no type checks
// ─────────────────────────────────────────────────────────────
Console.WriteLine("\n--- Items ---");

List<LibraryItem> items = new List<LibraryItem>();
items.Add(book);
items.Add(dvd);
items.Add(magazine);
foreach (LibraryItem item in items)
{
    Console.WriteLine($"{item.Title,-20} loan period: {item.LoanPeriodDays,2} days | daily late fee: {item.DailyLateFee:0.00}");
}

try
{
    head.ChangeLateFee(book, -1m);
}
catch (ArgumentException ex)
{
    Console.WriteLine("Rejected (late fee): " + ex.Message);
}

// ─────────────────────────────────────────────────────────────
// 4. Premium member returns a DVD 5 days late
// ─────────────────────────────────────────────────────────────
Console.WriteLine("\n--- Premium member returns a DVD 5 days late ---");

Loan dvdLoan = premium.Borrow(dvd, day1);
DateTime returnedOn = dvdLoan.DueDate.AddDays(5);
librarian.ProcessReturn(dvdLoan, returnedOn);

Console.WriteLine($"Loan {dvdLoan.LoanId}: borrowed {dvdLoan.BorrowDate:yyyy-MM-dd}, due {dvdLoan.DueDate:yyyy-MM-dd}, returned {returnedOn:yyyy-MM-dd}");
Console.WriteLine($"Days late: {dvdLoan.DaysLate}");
Console.WriteLine($"Late fee: {dvdLoan.LateFee:0.00}  (5 days × {dvd.DailyLateFee:0.00} − {premium.LateFeeDiscountPercent}% discount)");
Console.WriteLine($"Reading points: {premium.ReadingPoints}");
Console.WriteLine($"DVD on loan now? {dvd.IsOnLoan}");

// ─────────────────────────────────────────────────────────────
// 5. Status transitions — both must be rejected
// ─────────────────────────────────────────────────────────────
Console.WriteLine("\n--- Status transitions ---");

try
{
    librarian.ProcessReturn(dvdLoan, returnedOn);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected (return twice): " + ex.Message);
}

try
{
    librarian.MarkAsLost(dvdLoan);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected (lost after return): " + ex.Message);
}

// ─────────────────────────────────────────────────────────────
// 6. Extra validation rules
// ─────────────────────────────────────────────────────────────
Console.WriteLine("\n--- Extra validation ---");

try
{
    StudentMember nameless = new StudentMember("M-300", "", "0100");
}
catch (ArgumentException ex)
{
    Console.WriteLine("Rejected (empty name): " + ex.Message);
}

Loan magazineLoan = premium.Borrow(magazine, day1);
try
{
    librarian.ProcessReturn(magazineLoan, day1.AddDays(-2));
}
catch (ArgumentException ex)
{
    Console.WriteLine("Rejected (return before borrow): " + ex.Message);
}

Console.WriteLine($"\nFull loan history of {premium.FullName}:");
foreach (Loan loan in premium.Loans)
{
    Console.WriteLine($"  {loan.LoanId} | {loan.Item.Title,-26} | {loan.Status,-8} | fee {loan.LateFee:0.00}");
}

// ─────────────────────────────────────────────────────────────
// 8. Things that must NOT compile (uncomment any line to see the error)
// ─────────────────────────────────────────────────────────────
// Person p = new Person("P-1", "Nobody", "010");            // must NOT compile: protected constructor
// Member m = new Member("M-1", "Nobody", "010", 3, 0);      // must NOT compile: protected constructor
// Staff s = new Staff("S-1", "Nobody", "010", DateTime.Today, 5000, 0); // must NOT compile: protected constructor
// LibraryItem i = new LibraryItem("C-1", "X", 1, 7, 1);     // must NOT compile: protected constructor
// student.FullName = "New Name";                            // must NOT compile: FullName is get-only
// student.Loans.Add(null);                                  // must NOT compile: IReadOnlyList has no Add
// book.IsOnLoan = true;                                     // must NOT compile: private set
// dvdLoan.Status = LoanStatus.Returned;                        // must NOT compile: private set
// librarian.MonthlySalary = 99999;                          // must NOT compile: private set
