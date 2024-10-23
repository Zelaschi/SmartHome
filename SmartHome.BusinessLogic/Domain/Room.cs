using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class Room
{
    public Guid Id { get; set; }
    public required Home Home { get; set; }
    public List<HomeDevice>? HomeDevices { get; set; }
}
