using Microsoft.EntityFrameworkCore.Storage.Internal;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.WindowSensorModels.In;

public sealed class WindowSensorRequestModel
{
    public required string Name { get; set; }
    public required string ModelNumber { get; set; }
    public required string Description { get; set; }
    public required List<string> Photos { get; set; }
    public string? Type { get; set; } = DeviceTypesStatic.WindowSensor;

    public Device ToEntity()
    {
        return new Device
        {
            Id = Guid.NewGuid(),
            Type = Type,
            Name = Name,
            ModelNumber = ModelNumber,
            Description = Description,
            Photos = Photos.Select(path => new Photo { Id = Guid.NewGuid(), Path = path }).ToList()
        };
    }
}
