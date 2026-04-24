namespace EduConnect.Models;

public class Faculty : Person
{
    public string Department { get; set; } = string.Empty;

    public override Role GetRole() => Role.Faculty;
}
