namespace EduConnect.Models;

// SOLID: Open/Closed Principle (OCP) - Person is abstract, allowing extension (new roles) without modification.
// SOLID: Liskov Substitution Principle (LSP) - Any subclass of Person can be used where Person is expected.
public abstract class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Simulated password for login

    public abstract Role GetRole();
}
