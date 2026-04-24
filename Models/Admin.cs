namespace EduConnect.Models;

public class Admin : Person
{
    public override Role GetRole() => Role.Admin;
}
