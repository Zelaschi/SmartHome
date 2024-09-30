using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public class Device
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Type { get; set; } = "Window Sensor";
    public required string ModelNumber { get; set; }
    public required string Description { get; set; }
    public required string Photos { get; set; }
    public required Business Business { get; set; }
}
