using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IHomeLogic
{
    void AddDeviceToHome(Guid homeId, Guid deviceId);
    Home CreateHome(Home home);
    IEnumerable<Home> GetAllHomesByUserId(Guid userId);
    IEnumerable<HomeDevice> GetAllHomeDevices(Guid homeId);
    IEnumerable<HomeMember> GetAllHomeMembers(Guid homeId);

}
