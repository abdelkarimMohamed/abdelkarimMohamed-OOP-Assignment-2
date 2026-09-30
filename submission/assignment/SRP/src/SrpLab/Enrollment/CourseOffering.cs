namespace SrpLab.Enrollment;

/// <summary>Responsibility: describe the course being offered (code, seats, tuition).</summary>
public sealed record CourseOffering(string CourseCode, int Capacity, decimal Tuition);
