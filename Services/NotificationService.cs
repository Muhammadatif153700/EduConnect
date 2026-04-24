using EduConnect.Models;

namespace EduConnect.Services;

// SOLID: Single Responsibility Principle (SRP) - Only handles notification events and state
public class NotificationService
{
    private readonly List<Notification> _notifications = new();

    // Event-driven centerpiece
    public event Action<Notification>? OnNewNotification;

    public void SendNotification(Notification notification)
    {
        _notifications.Add(notification);
        // Fire event
        OnNewNotification?.Invoke(notification);
    }

    public IEnumerable<Notification> GetNotificationsForUser(Guid userId)
    {
        return _notifications.Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedAt);
    }

    public void MarkAsRead(Guid notificationId)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            // No need to fire event here unless we want to update unread count instantly everywhere,
            // but the bell component updates on click. We'll fire anyway just in case.
            OnNewNotification?.Invoke(notification);
        }
    }
    
    public int GetUnreadCount(Guid userId)
    {
        return _notifications.Count(n => n.UserId == userId && !n.IsRead);
    }
}
