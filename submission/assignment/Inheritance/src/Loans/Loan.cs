using LibrarySystem.Items;
using LibrarySystem.People;

namespace LibrarySystem.Loans;

/// <summary>
/// Links one member to one item. ID and borrow date are fixed at creation;
/// due date and late fee are always computed; status changes only through
/// MarkReturned / MarkLost, and only Borrowed → Returned or Borrowed → Lost.
/// </summary>
public class Loan
{
    private static int _lastLoanNumber;

    public string LoanId { get; }
    public DateTime BorrowDate { get; }
    public Member Member { get; }
    public LibraryItem Item { get; }
    public LoanStatus Status { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    public DateTime DueDate
    {
        get { return BorrowDate.AddDays(Item.LoanPeriodDays); }
    }

    public int DaysLate
    {
        get
        {
            if (!ReturnDate.HasValue)
                return 0;

            int days = (ReturnDate.Value.Date - DueDate.Date).Days;
            if (days < 0)
                return 0;
            return days;
        }
    }

    /// <summary>days late × item's daily late fee, minus the member's discount. Zero if on time.</summary>
    public decimal LateFee
    {
        get
        {
            if (DaysLate == 0)
                return 0m;

            decimal gross = DaysLate * Item.DailyLateFee;
            decimal discount = gross * Member.LateFeeDiscountPercent / 100m;
            return gross - discount;
        }
    }

    internal Loan(Member member, LibraryItem item, DateTime borrowDate)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        // Throws if the item is withdrawn or already on loan — before this loan exists.
        item.MarkAsBorrowed();

        _lastLoanNumber++;
        LoanId = "LN-" + _lastLoanNumber.ToString("D4");
        Member = member;
        Item = item;
        BorrowDate = borrowDate;
        Status = LoanStatus.Borrowed;
    }

    internal void MarkReturned(DateTime returnDate)
    {
        if (Status != LoanStatus.Borrowed)
            throw new InvalidOperationException($"Loan {LoanId} cannot be returned because it is already {Status}.");
        if (returnDate.Date < BorrowDate.Date)
            throw new ArgumentException(
                $"Return date {returnDate:yyyy-MM-dd} is earlier than borrow date {BorrowDate:yyyy-MM-dd}.");

        ReturnDate = returnDate;
        Status = LoanStatus.Returned;
        Item.MarkAsReturned();
    }

    internal void MarkLost()
    {
        if (Status != LoanStatus.Borrowed)
            throw new InvalidOperationException($"Loan {LoanId} cannot be marked as lost because it is already {Status}.");

        Status = LoanStatus.Lost;
    }
}
