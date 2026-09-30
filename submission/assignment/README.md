# SRP, Design Patterns & Inheritance — Simulation Academy .NET Diploma (Cycle 1)

| | |
|---|---|
| **Name** | Yahya — *add full name* |
| **Student ID** | *add your ID* |
| **Track** | Backend .NET |

## How to run

| Part | Command (from the repo root) |
|---|---|
| 01 — SRP | `dotnet run --project SRP/src/SrpLab.Runner` |
| 02 — Design Patterns | `dotnet run --project DesignPatterns/src/PatternsLab.Runner` |
| 03 — Inheritance | `dotnet run --project Inheritance/src` |
| 04 — LeetCode | `LeetCode/1456_MaxVowelsInSubstring/Solution.cs` (submitted on LeetCode) |

## Notes & assumptions

**Part 01 — SRP**
- Each original class is split into a folder/namespace (`Ward`, `Checkout`, `Support`, `Lending`, `Enrollment`, `Kitchen`, `Billing`, `Warehouse`, `Grading`, `Appointments`). The runner prints the same output as before, plus the export/secondary methods that the original runner never exercised.
- No interfaces were used. Small coordinator classes (`BedAdmission`, `TicketIntake`, `AppointmentScheduler`, `OrderTotalCalculator`) only orchestrate; the rules live in their own classes.
- `SubscriptionBilling`: composing the dunning email no longer mints an invoice number. `InvoiceNumberSequence` is the only place with that side effect, and it is an instance instead of a `static` field.

**Part 02 — Design Patterns**
- Singleton uses `Lazy<T>` with `ExecutionAndPublication` for thread safety; the constructor is private and the class is sealed.
- Prototype uses `MemberwiseClone()` (copies private `_modelData` without re-running the slow constructor), then deep-copies `Weapon` and `Abilities`. Bonus `EnemyRegistry` is included. `EnemyCopyHelper` was removed; clients call `enemy.Clone()`.
- Builder: required values (email, course, access mode) go in `CourseRegistration.For(...)`; everything else is an optional fluent step. The constructor is private, so `Build()` is the only way to get a registration, and it enforces the LiveGroup/VideosOnly rules. `AccessMode` became an enum instead of a magic string.

**Part 03 — Inheritance**
- Only taught features are used: no `abstract`, `virtual`/`override`, lambdas/`=>`, LINQ or custom interfaces. `Person`, `Member`, `Staff` and `LibraryItem` use protected constructors.
- Per-kind values (loan limit, discount, allowance, loan period, fee multiplier) are passed up via `: base(...)`, so `GetMonthlyPay()` and `DailyLateFee` live once in the parent with no type checks.
- Loan IDs are generated automatically (`LN-0001`, `LN-0002`, …) by a private static counter in `Loan`.
- `Member.Loans` returns `_loans.AsReadOnly()`, so the list cannot be cast back to `List<Loan>` and modified.
- State-changing methods on `LibraryItem` and `Loan` are `internal`; staff classes expose the public actions (`ProcessReturn`, `MarkAsLost`, `ChangeLateFee`, `WithdrawItem`, `RestoreItem`).
- **Assumption:** a lost item stays `IsOnLoan = true` (it never came back, so it cannot be lent again). A lost loan has a late fee of 0 — the request only defines fees for returned items.
- **Assumption:** the late fee uses the item's *current* daily fee when it is read.
- `ClassDiagram.dot` is the Graphviz source of `ClassDiagram.png`.
