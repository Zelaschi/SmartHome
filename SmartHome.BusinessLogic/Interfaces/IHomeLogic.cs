using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IHomeLogic
{
    IEnumerable<User> UnRelatedHomeOwners(Guid homeId);
    HomeDevice AddDeviceToHome(Guid homeId, Guid deviceId);
    Home CreateHome(Home home, Guid? userId);
    IEnumerable<Home> GetAllHomesByUserId(Guid userId);
    IEnumerable<HomeDevice> GetAllHomeDevices(Guid homeId, string? room);
    IEnumerable<HomeMember> GetAllHomeMembers(Guid homeId);
    HomeMember AddHomeMemberToHome(Guid homeId, Guid userId);
    void UpdateHomeDeviceName(Guid homeDeviceId, string newName);
    void UpdateHomeName(Guid homeId, string newName);
    Home GetHomeById(Guid id);
    bool TurnOnOffHomeDevice(Guid homeDeviceId);
}
