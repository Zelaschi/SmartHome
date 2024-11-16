using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IRoomLogic
{
    Room CreateRoom(Room room, Guid homeId);
    HomeDevice AddDevicesToRoom(Guid homeDeviceId, Guid roomId);
    List<Room> GetAllRoomsFromHome(Guid homeId);
}
