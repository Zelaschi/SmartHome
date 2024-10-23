using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.SecurityCameraModels.Out;

namespace SmartHome.WebApi.WebModels.WindowSensorModels.Out;

public class WindowSensorResponseModel
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "Window Sensor";
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string Photos { get; set; }
    public bool Open { get; set; }
    public string? Company { get; set; }

    public WindowSensorResponseModel(WindowSensor sensor)
    {
        Id = sensor.Id;
        Type = sensor.Type;
        Name = sensor.Name;
        ModelNumber = sensor.ModelNumber;
        Description = sensor.Description;
        Photos = sensor.Photos;
        Open = sensor.Open;
        Company = sensor.Business.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is WindowSensorResponseModel d && d.Id == Id;
    }
}
