namespace SrpLab.Checkout;

/// <summary>Responsibility: interpret marketing coupon codes and turn them into a discount amount.</summary>
public sealed class CouponDiscountCalculator
{
    public decimal DiscountFor(string? couponText, decimal subTotal)
    {
        if (string.IsNullOrWhiteSpace(couponText)) return 0m;
        var t = couponText.Trim().ToUpperInvariant();
        if (t.StartsWith("SAVE") && int.TryParse(t[4..], out var pct) && pct is > 0 and <= 50)
            return Math.Round(subTotal * pct / 100m, 2);
        if (t.Contains("FREESHIP")) return 0m; // shipping is handled elsewhere
        if (t == "WELCOME10") return Math.Min(10m, subTotal);
        return 0m;
    }
}
