using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.HomeDeviceModels.Out;

namespace SmartHome.WebApi.WebModels.HomeDeviceModels.Out;

public sealed class HomeDeviceResponseModel
{
    public Guid? HardwardId { get; set; }
    public bool Online { get; set; }
    public Device Device { get; set; }
    public bool? IsOn { get; set; }
    public bool? Open { get; set; }

    public HomeDeviceResponseModel(HomeDevice homeDevice)
    {
        HardwardId = homeDevice.Id;
        Online = homeDevice.Online;
        Device = homeDevice.Device;

        if (homeDevice.Device is InteligentLamp lamp)
        {
            IsOn = lamp.IsOn;
        }
        else if (homeDevice.Device is WindowSensor sensor)
        {
            Open = sensor.Open;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is HomeDeviceResponseModel h && h.HardwardId == HardwardId;
    }
}
