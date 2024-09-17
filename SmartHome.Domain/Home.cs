using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain;
public class Home
{
    public string MainStreet { get; set; }
    public string DoorNumber { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public int MaxMembers { get; set; }
    public User Owner { get; set; }
    public List<HomeDevice> Devices { get; set; }
    public List<HomeMember> Members { get; set; }

    public Home()
    {
        Devices = new List<HomeDevice>();
        Members = new List<HomeMember>();
    }
}
