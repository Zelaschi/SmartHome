using Microsoft.EntityFrameworkCore.Storage.Internal;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.WindowSensorModels.In;

public sealed class WindowSensorRequestModel
{
    public required string Name { get; set; }
    public required string ModelNumber { get; set; }
    public required string Description { get; set; }
    public required string Photos { get; set; }
    public bool Open { get; set; } = true;
    public string? Type { get; set; } = "Window Sensor";

    public WindowSensor ToEntity()
    {
        return new WindowSensor
        {
            Type = Type,
            Open = Open,
            Name = Name,
            ModelNumber = ModelNumber,
            Description = Description,
            Photos = Photos
        };
    }
}
