using EduConnect.Models;

namespace EduConnect.Services;

public interface ICourseService
{
    IEnumerable<Course> GetAllCourses();
    Course? GetCourseById(Guid id);
    void AddCourse(Course course);
    void UpdateCourse(Course course);
    void DeleteCourse(Guid id);
    
    IEnumerable<Course> GetCoursesByFaculty(Guid facultyId);
    
    // Enrollment
    void EnrollStudent(Guid studentId, Guid courseId);
    void DropCourse(Guid studentId, Guid courseId);
    IEnumerable<Enrollment> GetStudentEnrollments(Guid studentId);
    IEnumerable<Student> GetEnrolledStudents(Guid courseId);
    
    event Action<Guid>? OnEnrollmentChanged; // Broadcasts to NavBar to update credits
}
