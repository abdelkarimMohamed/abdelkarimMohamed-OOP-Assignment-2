using SrpLab.Appointments;
using SrpLab.Billing;
using SrpLab.Checkout;
using SrpLab.Enrollment;
using SrpLab.Grading;
using SrpLab.Kitchen;
using SrpLab.Lending;
using SrpLab.Support;
using SrpLab.Warehouse;
using SrpLab.Ward;

Console.WriteLine("SrpLab — 10 classes, refactored for SRP");
Console.WriteLine("=======================================");

// 1. WardBoard
var ward = new WardBoard();
var escalation = new EscalationPolicy();
var pager = new PagerLog();
var admission = new BedAdmission(ward, new AcuityScorer(), escalation, pager);
admission.Admit(1, "p-88", heartRate: 130, spo2: 89);
Console.WriteLine(new HandoffNoteWriter(escalation).Write(ward, 1));
Console.WriteLine(string.Join(" | ", pager.Drain()));
Console.WriteLine(new CensusCsvExporter().Export(ward));

// 2. CheckoutBasket
var basket = new CheckoutBasket();
basket.AddLine("SKU-1", 40m, 2);
var options = new CheckoutOptions();
options.ApplyCouponText("SAVE10");
options.EnableGiftWrap();
var total = new OrderTotalCalculator(new CouponDiscountCalculator(), new GiftWrapPricing()).GrandTotal(basket, options);
var auth = new PaymentAuthorizer().Authorize(total, "4242", basket.Lines.Count);
Console.WriteLine($"basket total={total} auth={auth}");

// 3. SupportTicket
var intake = new TicketIntake(new TicketPriorityClassifier());
var sla = new SlaPolicy();
var ticket = intake.Open("T-1", "cannot login", "prod is down for me", DateTimeOffset.UtcNow);
Console.WriteLine(new PublicReplyDrafter().Draft(ticket, sla.DeadlineFor(ticket), "Nora"));
Console.WriteLine(new EscalationBlurbWriter().Write(ticket, sla.DeadlineFor(ticket)));

// 4. LoanDesk
var application = new LoanApplication(60_000m, 640, 4, hasCollateral: false);
var risk = new RiskScorer();
var eligible = new EligibilityRule(risk).IsEligible(application);
var docs = new RequiredDocumentsChecklist().For(application, eligible);
Console.WriteLine(new DecisionLetterWriter().Write("Omar", application.RequestedAmount, risk.Score(application), eligible, docs));
Console.WriteLine(new UnderwriterCsvExporter().Row("APP-1", application, risk.Score(application), eligible));

// 5. CourseEnrollmentDesk
var course = new CourseOffering("SEF-101", Capacity: 1, Tuition: 3000m);
var roster = new CourseRoster(course);
Console.WriteLine(roster.Register("a@mail.com"));
Console.WriteLine(roster.Register("b@mail.com"));
Console.WriteLine(new WelcomePacketWriter().Write(course, roster, "b@mail.com", "Bea"));
Console.WriteLine(new TuitionInvoiceLineWriter(new TuitionVat()).Write(course, roster, "a@mail.com"));

// 6. KitchenTicket
var kitchen = new KitchenTicket();
kitchen.AddItem("Pasta", new[] { "wheat", "milk" }, 12);
var allergenDetector = new AllergenDetector();
var allergens = allergenDetector.Detect(kitchen);
var eta = new ReadyTimeEstimator(allergenDetector).EstimateMinutes(kitchen, openStations: 2);
Console.WriteLine(new ThermalTicketRenderer().Render(42, kitchen, eta, allergens));
Console.WriteLine(new ExpoLaneRouter().LaneFor(allergens.Count > 0, eta));

// 7. SubscriptionBilling
var sub = new Subscription("c-9", 99m, new DateOnly(2026, 9, 1), new DateOnly(2026, 10, 1));
sub.RegisterFailedPayment();
var proration = new ProrationCalculator();
var invoices = new InvoiceNumberSequence();
var invoiceNo = invoices.Next(sub.PeriodStart); // the side effect is now explicit and visible here
Console.WriteLine(new DunningEmailWriter(new DunningSeverityPolicy())
    .Write(sub, "Sara", proration.Prorate(sub, sub.PeriodStart), invoiceNo, new DateOnly(2026, 9, 20)));
Console.WriteLine(new LedgerJournalLineWriter().Write(sub, invoiceNo, proration.Prorate(sub, new DateOnly(2026, 9, 16))));

// 8. WarehousePickList
var pick = new PickList();
pick.AddNeed("BOLT", "A", 3, 10, 7);
pick.AddNeed("NUT", "B", 1, 5, 5);
var allocations = new StockAllocator().Allocate(pick);
var route = new PickRoutePlanner().Plan(allocations);
Console.WriteLine(new PickerScriptWriter().Write(route, allocations));
Console.WriteLine(new WmsXmlBatchExporter().Export("B-1", allocations));

// 9. GradeBook
var grades = new GradeBook();
grades.Record("s1", 92);
grades.Record("s1", 88);
var letters = new LetterGradePolicy();
var honor = new HonorRollPolicy(letters);
Console.WriteLine(new TranscriptWriter(letters, honor).Write(grades, "s1", "Ali"));
Console.WriteLine(new GradeCsvExporter(letters, honor).Export(grades));

// 10. AppointmentDesk
var hours = new BusinessHours(new TimeOnly(9, 0), new TimeOnly(17, 0), 30);
var scheduler = new AppointmentScheduler(hours, new AppointmentBook());
var slot = scheduler.FindNextSlot(DateTimeOffset.Parse("2026-09-21T08:00:00Z"), 48);
if (slot is null) throw new InvalidOperationException("no slot");
scheduler.TryBook(slot.Value);
Console.WriteLine(new SmsReminderWriter().Write(slot.Value, "0100"));
Console.WriteLine(new IcsCalendarWriter().Write(slot.Value, hours.SlotMinutes, "Mona", "Dr. Adel"));

Console.WriteLine("Done. Same behavior — one reason to change per class.");
