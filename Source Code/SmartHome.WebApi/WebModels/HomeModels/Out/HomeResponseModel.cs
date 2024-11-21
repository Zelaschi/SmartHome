using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeModels.Out;

public sealed class HomeResponseModel
{
    public Guid Id { get; set; }
    public string MainStreet { get; set; }
    public string DoorNumber { get; set; }
    public string Name { get; set; }

    public HomeResponseModel(Home home)
    {
        Id = home.Id;
        MainStreet = home.MainStreet;
        DoorNumber = home.DoorNumber;
        Name = home.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is HomeResponseModel d && d.Id == Id;
    }
}
