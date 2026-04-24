using EduConnect.Models;

namespace EduConnect.Services;

public interface IGradeService
{
    void SubmitGrade(GradeRecord gradeRecord);
    IEnumerable<GradeRecord> GetGradesForStudent(Guid studentId);
    IEnumerable<GradeRecord> GetGradesForCourse(Guid courseId);
    GradeRecord? GetGradeForStudentInCourse(Guid studentId, Guid courseId);
    
    event Action<Guid>? OnGradesSubmitted; // Guid is CourseId
}
