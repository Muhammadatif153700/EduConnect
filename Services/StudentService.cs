using EduConnect.Data;
using EduConnect.Exceptions;
using EduConnect.Models;

namespace EduConnect.Services;

// SOLID: Single Responsibility Principle (SRP) - Manages only Student logic.
public class StudentService : IStudentService
{
    private readonly IRepository<Student> _studentRepo;
    private readonly IRepository<Enrollment> _enrollmentRepo;

    // SOLID: Dependency Inversion Principle (DIP) - Injected interfaces, not concrete classes.
    public StudentService(IRepository<Student> studentRepo, IRepository<Enrollment> enrollmentRepo)
    {
        _studentRepo = studentRepo;
        _enrollmentRepo = enrollmentRepo;
    }

    public void AddStudent(Student student)
    {
        _studentRepo.Add(student);
    }

    public void DeleteStudent(Guid id)
    {
        var activeEnrollments = _enrollmentRepo.Find(e => e.StudentId == id && e.IsActive).Any();
        if (activeEnrollments)
        {
            throw new StudentHasActiveEnrollmentsException("Cannot delete student with active enrollments.");
        }
        
        _studentRepo.Delete(id);
    }

    public IEnumerable<Student> GetAllStudents()
    {
        return _studentRepo.GetAll();
    }

    public Student? GetStudentById(Guid id)
    {
        return _studentRepo.GetById(id);
    }

    public IEnumerable<Student> SearchStudents(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return GetAllStudents();
            
        return _studentRepo.Find(s => s.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                      s.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    public event Action<Student>? OnStudentUpdated;

    public void UpdateStudent(Student student)
    {
        _studentRepo.Update(student);
        OnStudentUpdated?.Invoke(student);
    }
}
