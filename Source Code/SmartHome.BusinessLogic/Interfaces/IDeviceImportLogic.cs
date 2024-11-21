using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IDeviceImportLogic
{
    int ImportDevices(string dllName, string fileName, User user);
}
