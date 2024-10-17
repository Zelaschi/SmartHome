using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.SecurityCameraModels.Out;

namespace SmartHome.WebApi.WebModels.WindowSensorModels.Out;

public class CreateWindowSensorResponseModel
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "Window Sensor";
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string Photos { get; set; }
    public bool Open { get; set; }
    public Business? Company { get; set; }

    public CreateWindowSensorResponseModel(WindowSensor sensor)
    {
        Id = sensor.Id;
        Type = sensor.Type;
        Name = sensor.Name;
        ModelNumber = sensor.ModelNumber;
        Description = sensor.Description;
        Photos = sensor.Photos;
        Open = sensor.Open;
        Company = sensor.Business;
    }

    public override bool Equals(object? obj)
    {
        return obj is CreateWindowSensorResponseModel d && d.Id == Id;
    }
}
