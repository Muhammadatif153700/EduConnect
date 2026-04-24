namespace EduConnect.Models;

public enum Role
{
    Admin,
    Faculty,
    Student
}

public enum NotificationType
{
    Enrollment,
    GradePosted,
    Announcement,
    System
}

public enum CourseStatus
{
    Open,
    AlmostFull,
    Full
}

public enum AlertType
{
    Success,
    Warning,
    Error,
    Info
}
