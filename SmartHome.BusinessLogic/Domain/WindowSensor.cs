    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class WindowSensor : Device
{
    public bool Open { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is WindowSensor sensor && sensor.Id == Id;
    }

    public WindowSensor()
    {
        Type = "Window Sensor";
    }
}
