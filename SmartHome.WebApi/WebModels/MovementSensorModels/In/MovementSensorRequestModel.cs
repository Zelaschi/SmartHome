using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.MovementSensorModels.In;

public sealed class MovementSensorRequestModel
{
    public required string Type { get; set; }
    public required string ModelNumber { get; set; }
    public required string Description { get; set; }
    public required string Name { get; set; }
    public required List<string> Photos { get; set; }

    public Device ToEntity()
    {
        return new Device()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Type = Type,
            ModelNumber = ModelNumber,
            Description = Description,
            Photos = Photos.Select(path => new Photo { Id = Guid.NewGuid(), Path = path }).ToList()
        };
    }
}
