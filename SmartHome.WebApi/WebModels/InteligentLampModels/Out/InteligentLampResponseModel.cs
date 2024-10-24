using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.InteligentLampModels.Out;

public class InteligentLampResponseModel
{
    public Guid Id { get; set; }
    public string? Type { get; set; }
    public string? ModelNumber { get; set; }
    public string? Description { get; set; }
    public List<Photo> Photos { get; set; }
    public bool? IsOn { get; set; }
    public string? Company { get; set; }
    public InteligentLampResponseModel(InteligentLamp lamp)
    {
        Id = lamp.Id;
        Type = lamp.Type;
        ModelNumber = lamp.ModelNumber;
        Description = lamp.Description;
        Photos = lamp.Photos;
        IsOn = lamp.IsOn;
        Company = lamp.Business.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is InteligentLampResponseModel d && d.Id == Id;
    }
}
