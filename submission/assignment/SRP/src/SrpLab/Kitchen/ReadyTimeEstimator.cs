namespace SrpLab.Kitchen;

/// <summary>Responsibility: the kitchen-operations model that estimates when an order is ready.</summary>
public sealed class ReadyTimeEstimator
{
    private const int AllergyProtocolMinutes = 3;
    private readonly AllergenDetector _allergens;

    public ReadyTimeEstimator(AllergenDetector allergens) => _allergens = allergens;

    public int EstimateMinutes(KitchenTicket ticket, int openStations)
    {
        if (openStations <= 0) openStations = 1;
        var items = ticket.Items;
        var sequential = items.Sum(i => i.PrepMinutes);
        var parallel = (int)Math.Ceiling(sequential / (double)openStations);
        if (_allergens.Detect(ticket).Count > 0) parallel += AllergyProtocolMinutes;
        var longest = items.Count == 0 ? 0 : items.Max(i => i.PrepMinutes);
        return Math.Max(parallel, longest);
    }
}
