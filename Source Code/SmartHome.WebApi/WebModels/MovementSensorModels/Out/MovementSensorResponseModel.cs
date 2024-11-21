using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.MovementSensorModels.Out;

public sealed class MovementSensorResponseModel
{
    public Guid Id { get; set; }
    public string? Type { get; set; }
    public string? ModelNumber { get; set; }
    public string? Description { get; set; }
    public List<string> Photos { get; set; }
    public string? Company { get; set; }

    public MovementSensorResponseModel(Device movementSensor)
    {
        Type = movementSensor.Type;
        Id = movementSensor.Id;
        ModelNumber = movementSensor.ModelNumber;
        Description = movementSensor.Description;
        Photos = movementSensor.Photos.Select(photo => photo.Path).ToList();
        Company = movementSensor.Business.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is MovementSensorResponseModel d && d.Id == Id;
    }
}
