using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeDeviceModels.Out;

public sealed class HomeDeviceResponseModel
{
    public Guid? HardwareId { get; set; }
    public bool Online { get; set; }
    public Device Device { get; set; }

    public HomeDeviceResponseModel(HomeDevice homeDevice)
    {
        HardwareId = homeDevice.Id;
        Online = homeDevice.Online;
        Device = homeDevice.Device;
    }

    public override bool Equals(object? obj)
    {
        return obj is HomeDeviceResponseModel h && h.HardwareId == HardwareId;
    }
}
