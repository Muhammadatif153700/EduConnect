namespace EduConnect.Models;

public class Enrollment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    
    // The assignment mentions "dropped courses cannot be re-enrolled in the same semester"
    // We'll track if the enrollment is active.
    public bool IsActive { get; set; } = true;
    public DateTime? DroppedDate { get; set; }
}
