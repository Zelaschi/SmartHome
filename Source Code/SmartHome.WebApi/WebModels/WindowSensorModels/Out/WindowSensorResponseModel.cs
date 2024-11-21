using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.WindowSensorModels.Out;

public class WindowSensorResponseModel
{
    public Guid Id { get; set; }
    public string? Type { get; set; }
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public List<string> Photos { get; set; }
    public string? Company { get; set; }

    public WindowSensorResponseModel(Device sensor)
    {
        Id = sensor.Id;
        Type = sensor.Type;
        Name = sensor.Name;
        ModelNumber = sensor.ModelNumber;
        Description = sensor.Description;
        Photos = sensor.Photos.Select(photo => photo.Path).ToList();
        Company = sensor.Business.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is WindowSensorResponseModel d && d.Id == Id;
    }
}
