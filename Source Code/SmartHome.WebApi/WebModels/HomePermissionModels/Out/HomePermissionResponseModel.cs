using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomePermissionModels.Out;

public class HomePermissionResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public HomePermissionResponseModel(HomePermission homePermission)
    {
        Id = homePermission.Id;
        Name = homePermission.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is HomePermissionResponseModel d && d.Id == Id;
    }
}
