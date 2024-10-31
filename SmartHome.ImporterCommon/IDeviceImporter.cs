using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ImporterCommon;
public interface IDeviceImporter
{
    public List<DTODevice> ImportDevicesFromFilePath(string path);
}
