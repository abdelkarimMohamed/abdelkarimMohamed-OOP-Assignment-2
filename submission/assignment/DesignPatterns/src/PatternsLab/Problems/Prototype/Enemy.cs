namespace PatternsLab.Problems.Prototype;

public class Weapon
{
    public string Name { get; set; } = "";
    public int Damage { get; set; }

    public Weapon Clone() => new() { Name = Name, Damage = Damage };
}

/// <summary>
/// Prototype: an Enemy knows how to copy itself.
/// MemberwiseClone() copies every field — including private _modelData — without running
/// the constructor, so the slow 3D-model load is skipped (R1, R3). It also keeps the runtime
/// type (an Orc clone is an Orc), so callers only see Enemy (R2). Reference-type members are then
/// replaced with fresh copies so the clone is independent (deep copy, R4).
/// </summary>
public abstract class Enemy
{
    private string _modelData;

    public string Name { get; set; } = "";
    public int Health { get; set; }
    public Weapon Weapon { get; set; } = new();
    public List<string> Abilities { get; set; } = new();
    public string ModelId => _modelData;

    protected Enemy()
    {
        Console.WriteLine("   ...loading 3D model (slow)...");
        Thread.Sleep(500);
        _modelData = "MODEL_" + Guid.NewGuid().ToString("N")[..6];
    }

    public virtual Enemy Clone()
    {
        var copy = (Enemy)MemberwiseClone();            // shallow: values + private fields, no ctor
        copy.Weapon = Weapon.Clone();                   // deep: own weapon
        copy.Abilities = new List<string>(Abilities);   // deep: own list
        return copy;
    }
}

public class Orc : Enemy
{
    public Orc()
    {
        Name = "Orc";
        Health = 100;
        Weapon = new Weapon { Name = "Axe", Damage = 25 };
        Abilities.Add("Rage");
    }
}

public class Elf : Enemy
{
    public Elf()
    {
        Name = "Elf";
        Health = 70;
        Weapon = new Weapon { Name = "Bow", Damage = 18 };
        Abilities.Add("Stealth");
    }
}
