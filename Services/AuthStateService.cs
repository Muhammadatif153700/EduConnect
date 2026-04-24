using EduConnect.Models;

namespace EduConnect.Services;

public class AuthStateService
{
    public Person? CurrentUser { get; private set; }
    
    public event Action? OnAuthStateChanged;
    
    public void Login(Person person)
    {
        CurrentUser = person;
        OnAuthStateChanged?.Invoke();
    }
    
    public void Logout()
    {
        CurrentUser = null;
        OnAuthStateChanged?.Invoke();
    }
    
    public bool IsLoggedIn => CurrentUser != null;
    public Role? CurrentRole => CurrentUser?.GetRole();
}
