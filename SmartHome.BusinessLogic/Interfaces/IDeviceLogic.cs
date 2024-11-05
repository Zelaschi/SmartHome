using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IDeviceLogic
{
    IEnumerable<Device> GetDevices(int? pageNumber, int? pageSize, string? deviceName, string? deviceModel, string? businessName, string? deviceType);
    IEnumerable<string> GetAllDeviceTypes();
}
