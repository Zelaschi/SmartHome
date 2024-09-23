using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeOwnerModels.In;

public class HomeOwnerRequestModel
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ProfilePhoto { get; set; }
    public User ToEntitiy(Role role)
    {
        return new User()
        {
            Role = role,
            Name = Name,
            Surname = Surname,
            Password = Password,
            Email = Email,
            ProfilePhoto = ProfilePhoto
        };
    }
}
