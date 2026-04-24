using System.ComponentModel.DataAnnotations;

namespace EduConnect.Models;

public class Student : Person, IValidatable
{
    public int Semester { get; set; } = 1;
    public double CGPA { get; set; } = 0.0;

    public override Role GetRole() => Role.Student;

    public bool IsValid()
    {
        return GetValidationErrors().Count == 0;
    }

    public Dictionary<string, string> GetValidationErrors()
    {
        var errors = new Dictionary<string, string>();
        
        if (string.IsNullOrWhiteSpace(FullName))
            errors.Add(nameof(FullName), "Full Name is required.");
            
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            errors.Add(nameof(Email), "A valid Email is required.");
            
        if (Semester < 1 || Semester > 8)
            errors.Add(nameof(Semester), "Semester must be between 1 and 8.");
            
        if (CGPA < 0.0 || CGPA > 4.0)
            errors.Add(nameof(CGPA), "CGPA must be between 0.0 and 4.0.");

        return errors;
    }
}
