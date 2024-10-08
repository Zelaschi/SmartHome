using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.UserModels.Out;

public sealed class UserResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public DateTime CreationDate { get; set; }

    public UserResponseModel(User user)
    {
        Id = (Guid)user.Id;
        Name = user.Name;
        Surname = user.Surname;
        Email = user.Email;
        CreationDate = (DateTime)user.CreationDate;
    }

    public override bool Equals(object? obj)
    {
        return obj is UserResponseModel d && d.Id == Id;
    }
}
