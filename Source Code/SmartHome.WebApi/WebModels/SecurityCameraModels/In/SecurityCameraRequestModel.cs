using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.SecurityCameraModels.In;

public sealed class SecurityCameraRequestModel
{
    public required string Type { get; set; } = DeviceTypesStatic.SecurityCamera;
    public required string ModelNumber { get; set; }
    public required string Description { get; set; }
    public required string Name { get; set; }
    public required List<string> Photos { get; set; }
    public required bool InDoor { get; set; }
    public required bool OutDoor { get; set; }
    public required bool MovementDetection { get; set; }
    public required bool PersonDetection { get; set; }

    public SecurityCamera ToEntity()
    {
        return new SecurityCamera()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Type = Type,
            ModelNumber = ModelNumber,
            Description = Description,
            Photos = Photos.Select(path => new Photo { Id = Guid.NewGuid(), Path = path }).ToList(),
            Outdoor = OutDoor,
            Indoor = InDoor,
            MovementDetection = MovementDetection,
            PersonDetection = PersonDetection
        };
    }
}
