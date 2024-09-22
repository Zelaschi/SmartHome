using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public class Home
{
    public required string MainStreet { get; set; }
    public required string DoorNumber { get; set; }
    public required string Latitude { get; set; }
    public required string Longitude { get; set; }
    public required int MaxMembers { get; set; }
    public required User Owner { get; set; }
    public List<HomeDevice> Devices { get; set; }
    public List<HomeMember> Members { get; set; }

    public Home()
    {
        Devices = new List<HomeDevice>();
        Members = new List<HomeMember>();
    }
}
