# Task 1.1 — Responsibilities of the 10 Classes

For each class: the separate *reasons to change* I found, why keeping them together is a problem, and the types I split it into (Task 1.2).

---

## 1. WardBoard

| # | Responsibility | Who would ask for a change |
|---|---|---|
| 1 | Tracking which patient is in which bed (census state) | Ward operations |
| 2 | Clinical acuity scoring from heart rate / SpO2 | Clinical team |
| 3 | Escalation thresholds (page at ≥ 8, WATCH at ≥ 4) | Nursing policy |
| 4 | Paging — writing CODE-YELLOW entries, draining the log | Pager/on-call system |
| 5 | Handoff note wording and layout | Nurses / documentation |
| 6 | CSV census export | Reporting / integration |

**Why it's a problem:** `AssignBed` looks like a simple setter but hides a side effect — it scores the patient *and* fires a pager entry. Changing the pager threshold means editing the same method that stores bed data, and the thresholds (8 / 4) were duplicated between paging and the handoff "tone". A change to the note's format risks breaking clinical logic.

**Split into:** `WardBoard` (census), `AcuityScorer`, `EscalationPolicy`, `PagerLog`, `BedAdmission` (coordinates the admit workflow), `HandoffNoteWriter`, `CensusCsvExporter`, `BedAssignment` (record).

---

## 2. CheckoutBasket

| # | Responsibility |
|---|---|
| 1 | Holding basket lines and computing the subtotal |
| 2 | Parsing marketing coupon strings into a discount |
| 3 | Packaging-fee policy (gift wrap 4.99) |
| 4 | Final total calculation (subtotal − discount + fees) |
| 5 | Gift-card customer copy |
| 6 | Payment authorization (gateway stub) |

**Why it's a problem:** Marketing adds a new coupon format → the cart class changes. The gateway changes its auth scheme → the cart class changes. The gift-card text depends on `GrandTotal()`, so a copy edit sits next to pricing math. Six different teams edit one file.

**Split into:** `CheckoutBasket`, `CheckoutOptions` (coupon + gift-wrap choice), `CouponDiscountCalculator`, `GiftWrapPricing`, `OrderTotalCalculator`, `GiftMessageCardWriter`, `PaymentAuthorizer`.

---

## 3. SupportTicket

| # | Responsibility |
|---|---|
| 1 | Ticket data (id, subject, body, opened-at, priority) |
| 2 | Keyword-based priority classification |
| 3 | SLA policy (4 / 24 / 72 hours, breach check) |
| 4 | Public reply templates (CX tone) |
| 5 | Internal escalation message format |

**Why it's a problem:** The constructor and `AppendCustomerMessage` silently re-run the keyword heuristic, so the entity is coupled to a support-playbook rule that changes often. SLA engineering, CX writers and support leads all have reasons to edit this one class, and a wording change can accidentally touch the SLA deadline logic.

**Split into:** `SupportTicket` (data), `TicketPriorityClassifier`, `TicketIntake` (open ticket / add message + reclassify), `SlaPolicy`, `PublicReplyDrafter`, `EscalationBlurbWriter`.

---

## 4. LoanDesk

| # | Responsibility |
|---|---|
| 1 | Application data |
| 2 | Risk scoring formula |
| 3 | Eligibility decision (risk ≥ 55 and credit ≥ 580) |
| 4 | Compliance checklist of required documents |
| 5 | Decision-letter wording (legal/comms) |
| 6 | Underwriter CSV export schema |

**Why it's a problem:** The risk committee, compliance, legal and analytics each own a part. `RequiredDocuments()` and `DecisionLetter()` call `IsEligible()`, so a change in the risk model ripples into letter text and the export. Testing the formula means constructing a class full of formatting code.

**Split into:** `LoanApplication`, `RiskScorer`, `EligibilityRule`, `RequiredDocumentsChecklist`, `DecisionLetterWriter`, `UnderwriterCsvExporter`.

---

## 5. CourseEnrollmentDesk

| # | Responsibility |
|---|---|
| 1 | Course definition (code, capacity, tuition) |
| 2 | Seat allocation, waitlist and promotion |
| 3 | Welcome-packet Markdown content |
| 4 | VAT rule (14%) |
| 5 | Invoice line format |

**Why it's a problem:** Marketing changes the welcome text, finance changes the VAT rate or invoice layout, and operations changes the seating rules — all in the same type that owns the seat state. A tax change should never require re-testing waitlist logic.

**Split into:** `CourseOffering`, `CourseRoster`, `WelcomePacketWriter`, `TuitionVat`, `TuitionInvoiceLineWriter`.

---

## 6. KitchenTicket

| # | Responsibility |
|---|---|
| 1 | Holding the order's dishes |
| 2 | Allergen dictionary / detection (regulatory) |
| 3 | Ready-time estimation (kitchen operations, incl. +3 min allergy protocol) |
| 4 | Thermal-printer layout (width 32, separators) |
| 5 | Expo-lane routing |

**Why it's a problem:** A new printer vendor, a new allergen regulation and a new station model are unrelated, yet all land in one class. `RenderThermalTicket` even hard-codes `EstimatedReadyMinutes(2)`, so the printer layout secretly decides the number of open stations.

**Split into:** `KitchenTicket`, `AllergenDetector`, `ReadyTimeEstimator`, `ThermalTicketRenderer`, `ExpoLaneRouter` (the station count is now passed in by the caller).

---

## 7. SubscriptionBilling

| # | Responsibility |
|---|---|
| 1 | Subscription terms + failed-payment count |
| 2 | Proration math (finance calendar) |
| 3 | Invoice number minting (sequence + fiscal prefix) |
| 4 | Dunning severity ladder (collections policy) |
| 5 | Dunning email wording |
| 6 | Ledger journal export line |

**Why it's a problem:** The worst hidden side effect in the lab: `DunningEmail()` calls `NextInvoiceNumber()`, so *just composing an email* consumes an invoice number, and `LedgerJournalLine()` consumes another one — the email and the ledger end up with different numbers for the same charge. The sequence is also a `static` field shared by every instance.

**Split into:** `Subscription`, `ProrationCalculator`, `InvoiceNumberSequence` (the only place that mints numbers, as an instance), `DunningSeverityPolicy`, `DunningEmailWriter` (receives the invoice number), `LedgerJournalLineWriter`.

---

## 8. WarehousePickList

| # | Responsibility |
|---|---|
| 1 | Holding the lines to pick |
| 2 | Stock allocation / backorder policy |
| 3 | Walking-path heuristic (aisle → bin) |
| 4 | Picker instructions (handheld UX wording) |
| 5 | WMS XML integration contract |

**Why it's a problem:** Allocation logic was duplicated — `WalkingOrder()` recomputes `Math.Min(need, onHand)` instead of using `Allocate()`, and `PickerScript()` re-finds the needed quantity with `First(l => l.Sku == ...)` (wrong if a SKU appears twice). A layout change, a WMS schema change and a backorder rule change all hit one file.

**Split into:** `PickList`, `StockAllocator` (+ `Allocation` record with `IsShort`), `PickRoutePlanner`, `PickerScriptWriter`, `WmsXmlBatchExporter`.

---

## 9. GradeBook

| # | Responsibility |
|---|---|
| 1 | Storing scores and computing averages |
| 2 | Letter-band policy (A/B/C/D/F) |
| 3 | Honor-roll rule |
| 4 | Transcript text format |
| 5 | CSV export format |

**Why it's a problem:** The faculty senate changes grade bands, the registrar changes the transcript, and IT changes the export — none of these should touch how scores are stored. The honor-roll rule is embedded next to averaging even though it is a separate academic policy.

**Split into:** `GradeBook`, `LetterGradePolicy`, `HonorRollPolicy`, `TranscriptWriter`, `GradeCsvExporter`.

---

## 10. AppointmentDesk

| # | Responsibility |
|---|---|
| 1 | Opening-hours policy (Fri/Sat closed, open/close time) |
| 2 | Tracking booked slots |
| 3 | Slot search and booking decisions |
| 4 | ICS calendar serialization |
| 5 | SMS reminder wording |

**Why it's a problem:** It is presented as "the scheduler", but calendar-format compatibility and SMS copy are messaging concerns owned by different people. Changing the working week (HR/ops) should not share a file with the ICS format that changes when calendar clients change.

**Split into:** `BusinessHours`, `AppointmentBook`, `AppointmentScheduler`, `IcsCalendarWriter`, `SmsReminderWriter`.
