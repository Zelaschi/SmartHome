using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain;
public class SecurityCamera : Device
{
    public bool Outdoor { get; set; }
    public bool Indoor { get; set; }
    public bool MovementDetection { get; set; }
    public bool PersonDetection { get; set; }
}
