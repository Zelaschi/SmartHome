using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.BusinessOwnerModels.Out;

public sealed class BusinessOwnerResponseModel
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Guid? Id { get; set; }
    public string? Role { get; set; }

    public BusinessOwnerResponseModel(User homeOwner)
    {
        Name = homeOwner.Name;
        Surname = homeOwner.Surname;
        Email = homeOwner.Email;
        Password = homeOwner.Password;
        Id = homeOwner.Id;
        Role = homeOwner.Role.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is BusinessOwnerResponseModel d && d.Id == Id;
    }
}
