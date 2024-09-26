using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.DeviceModels.Out;

namespace SmartHome.WebApi.WebModels.Businesses.Out;

public class BusinessesResponseModel
{
    public string Id;
    public string Name;
    public string OwnerName;
    public string Email;
    public string RUT;

    public BusinessesResponseModel(Company aCompany)
    {
        Id = aCompany.Id.ToString();
        Name = aCompany.Name;
        OwnerName = aCompany.CompanyOwner.Name;
        Email = aCompany.CompanyOwner.Email;
        RUT = aCompany.RUT;
    }

    public override bool Equals(object? obj)
    {
        return obj is BusinessesResponseModel d && d.RUT == RUT;
    }
}
