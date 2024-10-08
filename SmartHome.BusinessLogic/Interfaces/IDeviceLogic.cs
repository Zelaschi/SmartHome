using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IDeviceLogic
{
    Device CreateDevice(Device device, User user);
    IEnumerable<Device> GetAllDevices();
    IEnumerable<string> GetAllDeviceTypes();
}
