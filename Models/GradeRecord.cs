namespace EduConnect.Models;

public class GradeRecord : IValidatable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    
    public double Marks { get; set; } = 0.0;
    
    // Requirement: letter grade computation as a property
    public string LetterGrade
    {
        get
        {
            if (Marks >= 85) return "A";
            if (Marks >= 70) return "B";
            if (Marks >= 55) return "C";
            if (Marks >= 45) return "D";
            return "F";
        }
    }
    
    public double GradePoint
    {
        get
        {
            return LetterGrade switch
            {
                "A" => 4.0,
                "B" => 3.0,
                "C" => 2.0,
                "D" => 1.0,
                _ => 0.0
            };
        }
    }

    public bool IsValid()
    {
        return GetValidationErrors().Count == 0;
    }

    public Dictionary<string, string> GetValidationErrors()
    {
        var errors = new Dictionary<string, string>();
        
        if (Marks < 0 || Marks > 100)
            errors.Add(nameof(Marks), "Marks must be between 0 and 100.");

        return errors;
    }
}
