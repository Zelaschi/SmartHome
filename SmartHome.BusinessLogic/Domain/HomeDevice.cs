using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public class HomeDevice
{
    public required Guid HardwardId { get; set; }
    public required bool Online { get; set; }
    public required Device Device { get; set; }
}
