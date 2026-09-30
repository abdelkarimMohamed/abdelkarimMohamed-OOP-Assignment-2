namespace SrpLab.Enrollment;

/// <summary>Responsibility: the tax rule applied to tuition.</summary>
public sealed class TuitionVat
{
    private const decimal Rate = 0.14m;

    public decimal On(decimal tuition) => Math.Round(tuition * Rate, 2);
}
