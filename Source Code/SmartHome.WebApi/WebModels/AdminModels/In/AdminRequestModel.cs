using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.AdminModels.In;

public sealed class AdminRequestModel
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public User ToEntitiy()
    {
        return new User()
        {
            Role = new Role() { Name = "blankRole" },
            Name = Name,
            Surname = Surname,
            Password = Password,
            Email = Email,
            CreationDate = DateTime.Today
        };
    }
}
