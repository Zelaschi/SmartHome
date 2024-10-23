using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class Home
{
    public Guid Id { get; set; }
    public required string MainStreet { get; set; }
    public required string DoorNumber { get; set; }
    public required string Latitude { get; set; }
    public required string Longitude { get; set; }
    public required int MaxMembers { get; set; }
    public required User Owner { get; set; }
    public required string Name { get; set; }
    public List<HomeDevice>? Devices { get; set; }
    public List<HomeMember> Members { get; set; }
    public List<Room>? Rooms { get; set; }

    public Home()
    {
        Devices = new List<HomeDevice>();
        Members = new List<HomeMember>();
        Rooms = new List<Room>();
    }
}
