namespace SrpLab.Ward;

/// <summary>One bed's current occupant and acuity score (plain data).</summary>
public sealed record BedAssignment(int Bed, string PatientId, int Acuity);
