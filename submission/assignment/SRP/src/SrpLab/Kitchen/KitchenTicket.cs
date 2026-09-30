namespace SrpLab.Kitchen;

public sealed record TicketItem(string Name, IReadOnlyList<string> Ingredients, int PrepMinutes);

/// <summary>Responsibility: hold the dishes that make up one kitchen order.</summary>
public sealed class KitchenTicket
{
    private readonly List<TicketItem> _items = new();

    public IReadOnlyList<TicketItem> Items => _items;

    public void AddItem(string item, IEnumerable<string> ingredients, int prepMinutes)
    {
        _items.Add(new TicketItem(item, ingredients.Select(i => i.Trim().ToLowerInvariant()).ToList(), prepMinutes));
    }
}
