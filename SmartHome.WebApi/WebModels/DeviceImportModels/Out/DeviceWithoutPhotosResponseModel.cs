using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.DeviceImportModels.Out;

public sealed class DeviceWithoutPhotosResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Type { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string BusinessName { get; set; }

    public DeviceWithoutPhotosResponseModel(Device device)
    {
        Id = device.Id;
        Name = device.Name;
        Type = device.Type;
        ModelNumber = device.ModelNumber;
        Description = device.Description;
        BusinessName = device.Business.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is DeviceWithoutPhotosResponseModel model && model.Id == Id;
    }
}
