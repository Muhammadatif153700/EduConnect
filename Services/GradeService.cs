using EduConnect.Data;
using EduConnect.Models;

namespace EduConnect.Services;

public class GradeService : IGradeService
{
    private readonly IRepository<GradeRecord> _gradeRepo;
    private readonly IRepository<Student> _studentRepo;
    private readonly IRepository<Course> _courseRepo;
    private readonly NotificationService _notificationService;

    public event Action<Guid>? OnGradesSubmitted;

    public GradeService(
        IRepository<GradeRecord> gradeRepo,
        IRepository<Student> studentRepo,
        IRepository<Course> courseRepo,
        NotificationService notificationService)
    {
        _gradeRepo = gradeRepo;
        _studentRepo = studentRepo;
        _courseRepo = courseRepo;
        _notificationService = notificationService;
    }

    public GradeRecord? GetGradeForStudentInCourse(Guid studentId, Guid courseId)
    {
        return _gradeRepo.Find(g => g.StudentId == studentId && g.CourseId == courseId).FirstOrDefault();
    }

    public IEnumerable<GradeRecord> GetGradesForCourse(Guid courseId)
    {
        return _gradeRepo.Find(g => g.CourseId == courseId);
    }

    public IEnumerable<GradeRecord> GetGradesForStudent(Guid studentId)
    {
        return _gradeRepo.Find(g => g.StudentId == studentId);
    }

    public void SubmitGrade(GradeRecord gradeRecord)
    {
        var existing = GetGradeForStudentInCourse(gradeRecord.StudentId, gradeRecord.CourseId);
        if (existing != null)
        {
            existing.Marks = gradeRecord.Marks;
            _gradeRepo.Update(existing);
        }
        else
        {
            _gradeRepo.Add(gradeRecord);
        }

        // Trigger notification
        var course = _courseRepo.GetById(gradeRecord.CourseId);
        _notificationService.SendNotification(new Notification
        {
            UserId = gradeRecord.StudentId,
            Type = NotificationType.GradePosted,
            Message = $"A grade has been posted for {course?.Code ?? "a course"}."
        });

        // Recalculate CGPA
        RecalculateCGPA(gradeRecord.StudentId);
        
        OnGradesSubmitted?.Invoke(gradeRecord.CourseId);
    }
    
    private void RecalculateCGPA(Guid studentId)
    {
        var student = _studentRepo.GetById(studentId);
        if (student == null) return;
        
        var grades = GetGradesForStudent(studentId).ToList();
        if (!grades.Any()) return;
        
        double totalGradePoints = 0;
        int totalCredits = 0;
        
        foreach (var grade in grades)
        {
            var course = _courseRepo.GetById(grade.CourseId);
            if (course != null)
            {
                totalGradePoints += grade.GradePoint * course.CreditHours;
                totalCredits += course.CreditHours;
            }
        }
        
        if (totalCredits > 0)
        {
            student.CGPA = Math.Round(totalGradePoints / totalCredits, 2);
            _studentRepo.Update(student);
        }
    }
}
