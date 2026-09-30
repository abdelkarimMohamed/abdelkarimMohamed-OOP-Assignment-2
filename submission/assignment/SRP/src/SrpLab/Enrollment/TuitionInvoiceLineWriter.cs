namespace SrpLab.Enrollment;

/// <summary>Responsibility: the finance invoice line format.</summary>
public sealed class TuitionInvoiceLineWriter
{
    private readonly TuitionVat _vat;

    public TuitionInvoiceLineWriter(TuitionVat vat) => _vat = vat;

    public string Write(CourseOffering course, CourseRoster roster, string studentEmail)
    {
        if (!roster.IsSeated(studentEmail)) return $"{course.CourseCode},WAITLIST,0.00";
        var vat = _vat.On(course.Tuition);
        return $"{course.CourseCode},TUITION,{course.Tuition:0.00},VAT,{vat:0.00},TOTAL,{(course.Tuition + vat):0.00}";
    }
}
