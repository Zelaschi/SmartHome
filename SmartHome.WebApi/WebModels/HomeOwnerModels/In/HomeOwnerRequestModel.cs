using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeOwnerModels.In;

public sealed class HomeOwnerRequestModel
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ProfilePhoto { get; set; }
    public User ToEntitiy()
    {
        return new User()
        {
            Role = new Role() { Name = "blankRole" },
            Name = Name,
            Surname = Surname,
            Password = Password,
            Email = Email,
            ProfilePhoto = ProfilePhoto
        };
    }
}
