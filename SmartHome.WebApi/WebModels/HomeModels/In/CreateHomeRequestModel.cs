using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeModels.In;

public sealed class CreateHomeRequestModel
{
    public required string MainStreet { get; set; }
    public required string DoorNumber { get; set; }
    public required string Latitude { get; set; }
    public required string Longitude { get; set; }
    public required int MaxMembers { get; set; }

    public Home ToEntity()
    {
        return new Home
        {
            Owner = null,
            MainStreet = MainStreet,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaxMembers = MaxMembers
        };
    }
}
