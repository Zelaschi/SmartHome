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
    Home CreateHome(Home home, Guid userId);
    IEnumerable<Home> GetAllHomesByUserId(Guid userId);
}
