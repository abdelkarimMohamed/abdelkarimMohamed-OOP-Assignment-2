namespace SrpLab.Ward;

/// <summary>Responsibility: format the nurse handoff note text.</summary>
public sealed class HandoffNoteWriter
{
    private readonly EscalationPolicy _policy;

    public HandoffNoteWriter(EscalationPolicy policy) => _policy = policy;

    public string Write(WardBoard board, int bed)
    {
        var a = board.Find(bed);
        if (a is null) return $"Bed {bed}: empty";
        return $"[HANDOFF {DateTime.UtcNow:yyyy-MM-dd}] Bed {bed} · {a.PatientId} · acuity={a.Acuity} · {_policy.StatusFor(a.Acuity)}";
    }
}
