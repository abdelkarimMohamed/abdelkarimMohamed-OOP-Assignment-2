using System.Diagnostics;
using PatternsLab.Problems.Builder;
using PatternsLab.Problems.Prototype;
using PatternsLab.Problems.Singleton;

// ───────────────────────────── SINGLETON ─────────────────────────────
Console.WriteLine("=== SINGLETON: AFTER ===\n");

var db = new DatabaseService();
var ui = new UiService();

db.Config.Theme = "Dark";
Console.WriteLine("Admin changed theme to Dark.");
ui.Render();

Console.WriteLine($"\nSame config object? {ReferenceEquals(db.Config, ui.Config)}");

// R4: hammer Instance from many threads — still one object, still one load.
var fromThreads = new AppConfig[50];
Parallel.For(0, fromThreads.Length, i => fromThreads[i] = AppConfig.Instance);
Console.WriteLine($"All 50 threads got the same instance? {fromThreads.All(c => ReferenceEquals(c, db.Config))}");
Console.WriteLine($"Times config was loaded from disk: {AppConfig.LoadCount}");

// R5 — must NOT compile (constructor is private):
// var hacked = new AppConfig();

// ───────────────────────────── PROTOTYPE ─────────────────────────────
Console.WriteLine("\n=== PROTOTYPE: AFTER ===\n");

var registry = new EnemyRegistry();
Console.WriteLine("Loading the prototypes once:");
registry.Register("orc", new Orc());
registry.Register("elf", new Elf());

var sw = Stopwatch.StartNew();
var army = new List<Enemy>();
for (int i = 1; i <= 5; i++)
{
    var orc = registry.Create("orc");
    orc.Name = $"Orc-{i}";
    army.Add(orc);
}
Console.WriteLine($"\nCloned 5 orcs in {sw.ElapsedMilliseconds} ms (no model reload)");

Enemy original = registry.Create("orc");
original.Name = "Boss Orc";
Enemy copy = original.Clone();   // client only knows Enemy — no "if (e is Orc)"

Console.WriteLine($"\nCopy type: {copy.GetType().Name}");
Console.WriteLine($"Original model id: {original.ModelId}");
Console.WriteLine($"Copy model id:     {copy.ModelId}");

copy.Weapon.Damage = 999;
Console.WriteLine("\nWe changed the COPY's weapon damage to 999.");
Console.WriteLine($"Original's weapon damage is still: {original.Weapon.Damage}");

copy.Abilities.Add("Fire Breath");
Console.WriteLine($"Original abilities: {string.Join(", ", original.Abilities)}");
Console.WriteLine($"Copy abilities:     {string.Join(", ", copy.Abilities)}");

// ────────────────────────────── BUILDER ──────────────────────────────
Console.WriteLine("\n=== BUILDER: AFTER ===\n");
Console.WriteLine(RegistrationCallSites.CreateLiveStudent());
Console.WriteLine(RegistrationCallSites.CreateVideosOnly());

Console.WriteLine("\nInvalid combinations fail at Build():");
try
{
    CourseRegistration.For("omar@mail.com", "SEF-101", AccessMode.LiveGroup).Build();
}
catch (InvalidOperationException ex) { Console.WriteLine($"  ✗ {ex.Message}"); }

try
{
    CourseRegistration.For("mona@mail.com", "SEF-101", AccessMode.VideosOnly).InGroup("G2").Build();
}
catch (InvalidOperationException ex) { Console.WriteLine($"  ✗ {ex.Message}"); }

// must NOT compile (constructor is private — the Builder is the only way in):
// var raw = new CourseRegistration("x@mail.com", "SEF-101", ...);
