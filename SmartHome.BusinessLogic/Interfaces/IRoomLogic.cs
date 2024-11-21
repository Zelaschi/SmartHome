using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IRoomLogic
{
    Room CreateRoom(Room room, Guid homeId);
    HomeDevice AddDevicesToRoom(Guid homeDeviceId, Guid roomId);
    List<Room> GetAllRoomsFromHome(Guid homeId);
}
