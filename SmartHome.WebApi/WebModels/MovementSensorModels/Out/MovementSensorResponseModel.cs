using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.MovementSensorModels.Out;

public sealed class MovementSensorResponseModel
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "Sensor de Movimiento";
    public string? ModelNumber { get; set; }
    public string? Description { get; set; }
    public string? Photos { get; set; }
    public string? Company { get; set; }

    public MovementSensorResponseModel(Device movementSensor)
    {
        Id = movementSensor.Id;
        ModelNumber = movementSensor.ModelNumber;
        Description = movementSensor.Description;
        Photos = movementSensor.Photos;
        Company = movementSensor.Business.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is MovementSensorResponseModel d && d.Id == Id;
    }
}
