using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.DeviceImporter;
public sealed class DTOPhoto
{
    public required string Path { get; set; }
    public bool IsPrincipal { get; set; }
}
