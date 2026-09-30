namespace SrpLab.Checkout;

/// <summary>Responsibility: combine subtotal, discount and fees into the amount to pay.</summary>
public sealed class OrderTotalCalculator
{
    private readonly CouponDiscountCalculator _coupons;
    private readonly GiftWrapPricing _giftWrap;

    public OrderTotalCalculator(CouponDiscountCalculator coupons, GiftWrapPricing giftWrap)
    {
        _coupons = coupons;
        _giftWrap = giftWrap;
    }

    public decimal GrandTotal(CheckoutBasket basket, CheckoutOptions options)
    {
        var subTotal = basket.SubTotal();
        var total = subTotal - _coupons.DiscountFor(options.CouponText, subTotal);
        if (options.GiftWrap) total += _giftWrap.Fee;
        return Math.Max(0m, total);
    }
}
