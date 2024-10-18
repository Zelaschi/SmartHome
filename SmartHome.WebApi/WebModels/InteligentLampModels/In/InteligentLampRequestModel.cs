using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.InteligentLampModels.In;

public class InteligentLampRequestModel
{
    public required string Name { get; set; }
    public required string ModelNumber { get; set; }
    public required string Description { get; set; }
    public required string Photos { get; set; }
    public string? Type { get; set; } = "Inteligent Lamp";
    public required bool IsOn { get; set; }
    public InteligentLamp ToEntity()
    {
        return new InteligentLamp
        {
            IsOn = IsOn,
            Type = Type,
            Name = Name,
            ModelNumber = ModelNumber,
            Description = Description,
            Photos = Photos
        };
    }
}
