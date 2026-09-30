namespace SrpLab.Ward;

/// <summary>
/// Responsibility: run the admission workflow — score the vitals, place the patient, page if needed.
/// It only coordinates; each rule lives in its own class.
/// </summary>
public sealed class BedAdmission
{
    private readonly WardBoard _board;
    private readonly AcuityScorer _scorer;
    private readonly EscalationPolicy _policy;
    private readonly PagerLog _pager;

    public BedAdmission(WardBoard board, AcuityScorer scorer, EscalationPolicy policy, PagerLog pager)
    {
        _board = board;
        _scorer = scorer;
        _policy = policy;
        _pager = pager;
    }

    public BedAssignment Admit(int bed, string patientId, int heartRate, int spo2)
    {
        var acuity = _scorer.Score(heartRate, spo2);
        var assignment = _board.Assign(bed, patientId, acuity);
        if (_policy.RequiresPage(acuity)) _pager.FireCodeYellow(bed);
        return assignment;
    }
}
