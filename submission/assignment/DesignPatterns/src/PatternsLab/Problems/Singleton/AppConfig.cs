namespace PatternsLab.Problems.Singleton;

/// <summary>
/// Singleton: exactly one AppConfig for the whole app.
/// - sealed + private constructor  → nobody can "new" it or subclass it (R5)
/// - Lazy&lt;T&gt; (ExecutionAndPublication) → created on first use, once, thread-safe (R1, R3, R4)
/// - everyone reads AppConfig.Instance → same values everywhere (R2)
/// </summary>
public sealed class AppConfig
{
    private static readonly Lazy<AppConfig> _instance =
        new(() => new AppConfig(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static AppConfig Instance => _instance.Value;

    private static int _loadCount;
    public static int LoadCount => _loadCount;

    public string DbConnection { get; set; }
    public string Theme { get; set; }

    private AppConfig()
    {
        Interlocked.Increment(ref _loadCount);
        Console.WriteLine($"[AppConfig] Loading settings from disk... (load #{_loadCount})");
        Thread.Sleep(300);
        DbConnection = "Server=localhost;Db=School";
        Theme = "Light";
    }
}

public class DatabaseService
{
    public AppConfig Config { get; } = AppConfig.Instance;

    public void Connect() => Console.WriteLine($"Connecting to {Config.DbConnection}");
}

public class UiService
{
    public AppConfig Config { get; } = AppConfig.Instance;

    public void Render() => Console.WriteLine($"UI is using theme: {Config.Theme}");
}
