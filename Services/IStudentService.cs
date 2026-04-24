using EduConnect.Models;

namespace EduConnect.Services;

// SOLID: Interface Segregation Principle (ISP) - Specific interface for Student operations, independent of generic repository.
public interface IStudentService
{
    IEnumerable<Student> GetAllStudents();
    Student? GetStudentById(Guid id);
    void AddStudent(Student student);
    void UpdateStudent(Student student);
    void DeleteStudent(Guid id);
    IEnumerable<Student> SearchStudents(string searchTerm);
    
    event Action<Student>? OnStudentUpdated;
}
