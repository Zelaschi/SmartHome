using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.Businesses.In;

public sealed class BusinessRequestModel
{
    public required string Name { get; set; }
    public required string Logo { get; set; }
    public required string RUT { get; set; }
    public Business ToEntity()
    {
        return new Business()
        {
            Id = new Guid(),
            BusinessOwner = new User() { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = new Role() { Name = "blankRole" } },
            Name = Name,
            Logo = Logo,
            RUT = RUT
        };
    }
}
