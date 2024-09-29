using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.DeviceModels.Out;

namespace SmartHome.WebApi.WebModels.Businesses.Out;

public sealed class BusinessesResponseModel
{
    public string Id;
    public string Name;
    public string OwnerName;
    public string Email;
    public string RUT;

    public BusinessesResponseModel(Business aBusiness)
    {
        Id = aBusiness.Id.ToString();
        Name = aBusiness.Name;
        OwnerName = aBusiness.BusinessOwner.Name;
        Email = aBusiness.BusinessOwner.Email;
        RUT = aBusiness.RUT;
    }

    public override bool Equals(object? obj)
    {
        return obj is BusinessesResponseModel d && d.RUT == RUT;
    }
}
