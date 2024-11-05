using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.SecurityCameraModels.In;

public sealed class SecurityCameraRequestModel
{
    public string? Type { get; set; } = DeviceTypesStatic.SecurityCamera;
    public string? ModelNumber { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
    public List<Photo>? Photos { get; set; }
    public bool? InDoor { get; set; }
    public bool? OutDoor { get; set; }
    public bool? MovementDetection { get; set; }
    public bool? PersonDetection { get; set; }

    public SecurityCamera ToEntity()
    {
        return new SecurityCamera()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Type = Type,
            ModelNumber = ModelNumber,
            Description = Description,
            Photos = Photos,
            Outdoor = (bool)OutDoor,
            Indoor = (bool)InDoor,
            MovementDetection = (bool)MovementDetection,
            PersonDetection = (bool)PersonDetection
        };
    }
}
