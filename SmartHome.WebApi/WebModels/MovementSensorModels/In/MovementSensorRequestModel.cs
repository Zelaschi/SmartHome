using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.MovementSensorModels.In;

public sealed class MovementSensorRequestModel
{
    public string? Type { get; set; } = "Movement Sensor";
    public string? ModelNumber { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
    public List<Photo>? Photos { get; set; }

    public Device ToEntity()
    {
        return new Device()
        {
            Name = Name,
            Type = Type,
            ModelNumber = ModelNumber,
            Description = Description,
            Photos = Photos
        };
    }
}
