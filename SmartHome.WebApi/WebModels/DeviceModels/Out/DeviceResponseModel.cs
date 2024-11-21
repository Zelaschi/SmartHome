using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.DeviceModels.Out;
public sealed class DeviceResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public List<string> Photos { get; set; }
    public string CompanyName { get; set; }

    public DeviceResponseModel(Device device)
    {
        Id = device.Id;
        Name = device.Name;
        ModelNumber = device.ModelNumber;
        Description = device.Description;
        Photos = device.Photos.Select(photo => photo.Path).ToList();
        CompanyName = device.Business.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is DeviceResponseModel d && d.Id == Id;
    }
}
