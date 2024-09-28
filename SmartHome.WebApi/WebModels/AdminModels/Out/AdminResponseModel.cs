using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.Out;

namespace SmartHome.WebApi.WebModels.AdminModels.Out;

public sealed class AdminResponseModel
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Guid? Id { get; set; }
    public Role? Role { get; set; }

    public AdminResponseModel(User homeOwner)
    {
        Name = homeOwner.Name;
        Surname = homeOwner.Surname;
        Email = homeOwner.Email;
        Password = homeOwner.Password;
        Id = homeOwner.Id;
        Role = homeOwner.Role;
    }

    public override bool Equals(object? obj)
    {
        return obj is AdminResponseModel d && d.Id == Id;
    }
}
