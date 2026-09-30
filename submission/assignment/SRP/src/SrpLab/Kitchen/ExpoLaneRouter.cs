namespace SrpLab.Kitchen;

/// <summary>Responsibility: decide which expo lane an order goes to.</summary>
public sealed class ExpoLaneRouter
{
    private const int SlowOrderMinutes = 20;

    public string LaneFor(bool hasAllergens, int etaMinutes) =>
        hasAllergens ? "LANE-ALLERGY" : etaMinutes > SlowOrderMinutes ? "LANE-SLOW" : "LANE-FAST";
}
