using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class SecurityCamera : Device
{
    public bool Outdoor { get; set; }
    public bool Indoor { get; set; }
    public bool MovementDetection { get; set; }
    public bool PersonDetection { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is SecurityCamera camera && camera.Id == Id;
    }

    public SecurityCamera()
    {
        Type = "Security Camera";
    }
}
