using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.DeviceImporter;
public interface IDeviceImporter
{
    public List<DTODevice> ImportDevicesFromFilePath(string path);
}
