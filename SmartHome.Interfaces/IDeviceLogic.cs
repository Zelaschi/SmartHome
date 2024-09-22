using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Domain;

namespace SmartHome.Interfaces;
public interface IDeviceLogic
{
    User CreateDevice(Device device);
    IEnumerable<Device> GetAllDevices();
}
