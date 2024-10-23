using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.DeviceModels.Out;

namespace SmartHome.WebApi.WebModels.HomeOwnerModels.Out;

public sealed class HomeOwnerResponseModel
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ProfilePhoto { get; set; }
    public Guid? Id { get; set; }
    public string? Role { get; set; }

    public HomeOwnerResponseModel(User homeOwner)
    {
        Name = homeOwner.Name;
        Surname = homeOwner.Surname;
        Email = homeOwner.Email;
        Password = homeOwner.Password;
        ProfilePhoto = homeOwner.ProfilePhoto;
        Id = homeOwner.Id;
        Role = homeOwner.Role.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is HomeOwnerResponseModel d && d.Id == Id;
    }
}
