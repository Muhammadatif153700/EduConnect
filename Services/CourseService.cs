using EduConnect.Data;
using EduConnect.Exceptions;
using EduConnect.Models;

namespace EduConnect.Services;

public class CourseService : ICourseService
{
    private readonly IRepository<Course> _courseRepo;
    private readonly IRepository<Enrollment> _enrollmentRepo;
    private readonly IRepository<Student> _studentRepo;
    private readonly NotificationService _notificationService;

    public event Action<Guid>? OnEnrollmentChanged;

    public CourseService(
        IRepository<Course> courseRepo, 
        IRepository<Enrollment> enrollmentRepo,
        IRepository<Student> studentRepo,
        NotificationService notificationService)
    {
        _courseRepo = courseRepo;
        _enrollmentRepo = enrollmentRepo;
        _studentRepo = studentRepo;
        _notificationService = notificationService;
    }

    public void AddCourse(Course course)
    {
        _courseRepo.Add(course);
    }

    public void DeleteCourse(Guid id)
    {
        _courseRepo.Delete(id);
    }

    public void DropCourse(Guid studentId, Guid courseId)
    {
        var enrollment = _enrollmentRepo.Find(e => e.StudentId == studentId && e.CourseId == courseId && e.IsActive).FirstOrDefault();
        if (enrollment != null)
        {
            enrollment.IsActive = false;
            enrollment.DroppedDate = DateTime.Now;
            _enrollmentRepo.Update(enrollment);
            
            var course = _courseRepo.GetById(courseId);
            if (course != null)
            {
                course.CurrentEnrollment--;
                _courseRepo.Update(course);
            }
            
            OnEnrollmentChanged?.Invoke(studentId);
        }
    }

    public void EnrollStudent(Guid studentId, Guid courseId)
    {
        var course = _courseRepo.GetById(courseId);
        if (course == null) return;

        if (course.Status == CourseStatus.Full)
        {
            throw new CourseFullException($"Course {course.Code} is already full.");
        }
        
        // Business Rule: Cannot re-enroll if dropped in same semester (simplified here to just cannot re-enroll if dropped)
        var previousEnrollment = _enrollmentRepo.Find(e => e.StudentId == studentId && e.CourseId == courseId).FirstOrDefault();
        if (previousEnrollment != null && !previousEnrollment.IsActive)
        {
            throw new InvalidOperationException("Cannot re-enroll in a previously dropped course.");
        }
        
        if (previousEnrollment != null && previousEnrollment.IsActive)
        {
            // already enrolled
            return;
        }

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId
        };
        
        _enrollmentRepo.Add(enrollment);
        
        course.CurrentEnrollment++;
        _courseRepo.Update(course);
        
        // Trigger notification
        _notificationService.SendNotification(new Notification
        {
            UserId = studentId,
            Type = NotificationType.Enrollment,
            Message = $"You have successfully enrolled in {course.Code} - {course.Title}"
        });
        
        OnEnrollmentChanged?.Invoke(studentId);
    }

    public IEnumerable<Course> GetAllCourses()
    {
        return _courseRepo.GetAll();
    }

    public Course? GetCourseById(Guid id)
    {
        return _courseRepo.GetById(id);
    }

    public IEnumerable<Course> GetCoursesByFaculty(Guid facultyId)
    {
        return _courseRepo.Find(c => c.FacultyId == facultyId);
    }

    public IEnumerable<Student> GetEnrolledStudents(Guid courseId)
    {
        var studentIds = _enrollmentRepo.Find(e => e.CourseId == courseId && e.IsActive).Select(e => e.StudentId).ToList();
        return _studentRepo.Find(s => studentIds.Contains(s.Id));
    }

    public IEnumerable<Enrollment> GetStudentEnrollments(Guid studentId)
    {
        return _enrollmentRepo.Find(e => e.StudentId == studentId && e.IsActive);
    }

    public void UpdateCourse(Course course)
    {
        _courseRepo.Update(course);
    }
}
