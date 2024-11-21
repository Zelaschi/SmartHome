using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.Businesses.In;

public sealed class BusinessRequestModel
{
    public required string Name { get; set; }
    public required string Logo { get; set; }
    public required string RUT { get; set; }
    public string? ValidatorId { get; set; } = null;
    public Business ToEntity()
    {
        if (!string.IsNullOrEmpty(ValidatorId))
        {
            var validatorId = new Guid(ValidatorId);
            return new Business()
            {
                Id = new Guid(),
                Name = Name,
                Logo = Logo,
                RUT = RUT,
                ValidatorId = validatorId,
            };
        }
        else
        {
            return new Business()
            {
                Id = new Guid(),
                Name = Name,
                Logo = Logo,
                RUT = RUT,
            };
        }
    }
}
