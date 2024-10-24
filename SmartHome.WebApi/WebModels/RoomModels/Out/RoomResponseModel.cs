using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.RoomModels.Out;

public class RoomResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public RoomResponseModel(Room room)
    {
        Id = room.Id;
        Name = room.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is RoomResponseModel d && d.Id == Id;
    }
}
