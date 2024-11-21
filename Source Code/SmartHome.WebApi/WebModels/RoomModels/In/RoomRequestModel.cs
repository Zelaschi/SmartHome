using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.RoomModels.In;

public class RoomRequestModel
{
    public required string Name { get; set; }

    public Room ToEntity()
    {
        return new Room
        {
            Home = null,
            Name = Name
        };
    }
}
