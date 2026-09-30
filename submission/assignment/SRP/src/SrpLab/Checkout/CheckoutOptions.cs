namespace SrpLab.Checkout;

/// <summary>Responsibility: remember the shopper's checkout choices (coupon text, gift wrap).</summary>
public sealed class CheckoutOptions
{
    public string? CouponText { get; private set; }
    public bool GiftWrap { get; private set; }

    public void ApplyCouponText(string? couponText) => CouponText = couponText;
    public void EnableGiftWrap() => GiftWrap = true;
}
