namespace EduConnect.Models;

public interface IValidatable
{
    bool IsValid();
    Dictionary<string, string> GetValidationErrors();
}
