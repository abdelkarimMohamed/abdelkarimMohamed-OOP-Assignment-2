namespace SrpLab.Kitchen;

/// <summary>Responsibility: lay out the ticket for the thermal printer (width, separators, casing).</summary>
public sealed class ThermalTicketRenderer
{
    private const int Width = 32;

    public string Render(int orderNumber, KitchenTicket ticket, int etaMinutes, IReadOnlyList<string> allergens)
    {
        var line = new string('=', Width);
        var body = string.Join('\n', ticket.Items.Select(i => $"* {i.Name.ToUpperInvariant()} ({i.PrepMinutes}m)"));
        var allergyLine = allergens.Count == 0 ? "ALLERGENS: none" : "ALLERGENS: " + string.Join(",", allergens);
        return $"{line}\nORDER #{orderNumber}\nETA {etaMinutes} MIN\n{body}\n{allergyLine}\n{line}\n";
    }
}
