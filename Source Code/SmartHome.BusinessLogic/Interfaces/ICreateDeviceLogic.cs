using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface ICreateDeviceLogic
{
    Device CreateDevice(Device device, User user, string type);
}
