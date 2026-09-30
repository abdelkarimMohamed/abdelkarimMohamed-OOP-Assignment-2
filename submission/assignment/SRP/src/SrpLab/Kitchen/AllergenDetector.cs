namespace SrpLab.Kitchen;

/// <summary>Responsibility: the regulatory allergen dictionary — which ingredients map to which allergen.</summary>
public sealed class AllergenDetector
{
    public IReadOnlyList<string> Detect(KitchenTicket ticket)
    {
        var hits = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in ticket.Items)
        {
            foreach (var ing in item.Ingredients)
            {
                if (ing.Contains("milk") || ing.Contains("cheese") || ing.Contains("butter")) hits.Add("dairy");
                if (ing.Contains("wheat") || ing.Contains("flour") || ing.Contains("bread")) hits.Add("gluten");
                if (ing.Contains("peanut") || ing.Contains("almond") || ing.Contains("cashew")) hits.Add("nuts");
                if (ing.Contains("shrimp") || ing.Contains("prawn") || ing.Contains("crab")) hits.Add("shellfish");
            }
        }
        return hits.OrderBy(x => x).ToList();
    }
}
