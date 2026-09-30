namespace SrpLab.Checkout;

/// <summary>Responsibility: talk to the (stubbed) payment gateway and return an auth code.</summary>
public sealed class PaymentAuthorizer
{
    public string Authorize(decimal amount, string cardLast4, int lineCount)
    {
        var payload = $"{amount:0.00}|{cardLast4}|{lineCount}";
        var hash = payload.GetHashCode();
        return $"AUTH-{Math.Abs(hash):X8}";
    }
}
