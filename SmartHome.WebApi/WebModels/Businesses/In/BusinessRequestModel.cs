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
            Name = Name,
            Logo = Logo,
            RUT = RUT
        };
    }
}
