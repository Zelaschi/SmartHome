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
    public string? Type { get; set; }
    public required string ModelNumber { get; set; }
    public required string Description { get; set; }
    public required List<Photo> Photos { get; set; }
    public Business? Business { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Device device && device.Id == Id;
    }
}
