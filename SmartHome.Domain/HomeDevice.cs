using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain;
public class HomeDevice
{
    public required string HardwareId { get; set; }
    public required string Online { get; set; }
    public required string Device { get; set; }
}
