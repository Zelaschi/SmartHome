using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.Businesses.Out;

public sealed class BusinessesResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string OwnerName { get; set; }
    public string Email { get; set; }
    public string RUT { get; set; }
    public string? Logo { get; set; }

    public BusinessesResponseModel(Business aBusiness)
    {
        Id = aBusiness.Id.ToString();
        Name = aBusiness.Name;
        OwnerName = aBusiness.BusinessOwner.Name;
        Email = aBusiness.BusinessOwner.Email;
        RUT = aBusiness.RUT;
        if (aBusiness.Logo != null)
        {
            Logo = aBusiness.Logo;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is BusinessesResponseModel d && d.RUT == RUT;
    }
}
