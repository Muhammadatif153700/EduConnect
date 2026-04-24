using System.ComponentModel.DataAnnotations;

namespace EduConnect.Models;

public class Course : IValidatable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int CreditHours { get; set; } = 3;
    public int MaxCapacity { get; set; } = 30;
    
    // Assignment to Faculty
    public Guid? FacultyId { get; set; }
    
    // The requirement says:
    // "Use a computed property or method in the Course class to return EnrollmentStatus 
    // (enum: Open, AlmostFull, Full)" based on current enrollment vs MaxCapacity.
    // We will need a way to pass the current enrollment count to this method/property.
    // Or we store CurrentEnrollment as a property that gets updated by the service.
    // A simpler approach is just storing CurrentEnrollment.
    public int CurrentEnrollment { get; set; } = 0;

    public CourseStatus Status
    {
        get
        {
            if (CurrentEnrollment >= MaxCapacity) return CourseStatus.Full;
            if (CurrentEnrollment >= MaxCapacity - 5) return CourseStatus.AlmostFull;
            return CourseStatus.Open;
        }
    }

    public bool IsValid()
    {
        return GetValidationErrors().Count == 0;
    }

    public Dictionary<string, string> GetValidationErrors()
    {
        var errors = new Dictionary<string, string>();
        
        if (string.IsNullOrWhiteSpace(Code))
            errors.Add(nameof(Code), "Course Code is required.");
            
        if (string.IsNullOrWhiteSpace(Title))
            errors.Add(nameof(Title), "Course Title is required.");
            
        if (CreditHours < 1 || CreditHours > 4)
            errors.Add(nameof(CreditHours), "Credit Hours must be between 1 and 4.");
            
        if (MaxCapacity < 1)
            errors.Add(nameof(MaxCapacity), "Max Capacity must be at least 1.");

        return errors;
    }
}
