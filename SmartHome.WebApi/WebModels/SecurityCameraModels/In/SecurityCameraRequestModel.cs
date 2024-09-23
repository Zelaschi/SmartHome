using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.SecurityCameraModels.In;

public class SecurityCameraRequestModel
{
    public string Type { get; set; } = "Camara de Seguridad";
    public string? ModelNumber { get; set; }
    public string? Description { get; set; }
    public string? Photos { get; set; }
    public bool? InDoor { get; set; }
    public bool? OutDoor { get; set; }
    public bool? MovementDetection { get; set; }
    public bool? PersonDetection { get; set; }
    public Company? Company { get; set; }

    public SecurityCamera ToEntity()
    {
        return new SecurityCamera()
        {
            Name = "name??",
            Type = Type,
            ModelNumber = ModelNumber,
            Description = Description,
            Photos = Photos,
            Company = Company,
            Outdoor = (bool)OutDoor,
            Indoor = (bool)InDoor,
            MovementDetection = (bool)MovementDetection,
            PersonDetection = (bool)PersonDetection
        };
    }
}
