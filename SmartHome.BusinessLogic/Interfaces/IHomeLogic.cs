using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IHomeLogic
{
    HomeDevice AddDeviceToHome(Guid homeId, Guid deviceId);
    Home CreateHome(Home home, Guid? userId);
    IEnumerable<Home> GetAllHomesByUserId(Guid userId);
    IEnumerable<HomeDevice> GetAllHomeDevices(Guid homeId);
    IEnumerable<HomeMember> GetAllHomeMembers(Guid homeId);
    HomeMember AddHomeMemberToHome(Guid homeId, Guid userId);
}
