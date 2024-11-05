using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.HomeDeviceModels.Out;

namespace SmartHome.WebApi.WebModels.HomeDeviceModels.Out;

public sealed class HomeDeviceResponseModel
{
    public Guid? HardwardId { get; set; }
    public bool Online { get; set; }
    public string Device { get; set; }
    public string Type { get; set; }
    public bool? IsOn { get; set; }
    public bool? Open { get; set; }

    public HomeDeviceResponseModel(HomeDevice homeDevice)
    {
        Type = homeDevice.Device.Type;
        HardwardId = homeDevice.Id;
        Online = homeDevice.Online;
        Device = homeDevice.Device.Name;

        if (homeDevice.Device.Type is DeviceTypesStatic.InteligentLamp)
        {
            IsOn = homeDevice.IsOn;
        }
        else if (homeDevice.Device.Type is DeviceTypesStatic.WindowSensor)
        {
            Open = homeDevice.IsOpen;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is HomeDeviceResponseModel h && h.HardwardId == HardwardId;
    }
}
