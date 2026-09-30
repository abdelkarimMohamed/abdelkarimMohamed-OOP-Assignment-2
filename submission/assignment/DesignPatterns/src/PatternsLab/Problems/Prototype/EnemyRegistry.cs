namespace PatternsLab.Problems.Prototype;

/// <summary>R5 (bonus): stores named prototypes and hands out clones on demand.</summary>
public sealed class EnemyRegistry
{
    private readonly Dictionary<string, Enemy> _prototypes = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string key, Enemy prototype)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key is required.", nameof(key));
        _prototypes[key] = prototype ?? throw new ArgumentNullException(nameof(prototype));
    }

    public Enemy Create(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
            throw new KeyNotFoundException($"No enemy prototype registered as '{key}'.");
        return prototype.Clone();
    }
}
