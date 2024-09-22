using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IDeviceLogic
{
    User CreateDevice(Device device);
    IEnumerable<Device> GetAllDevices();
}
