using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Domain;

namespace SmartHome.WebModels.DeviceModels.In;
public sealed class CreateDeviceRequestModel
{
    public required string Name { get; set; }
    public required string ModelNumber { get; set; }
    public required string Description { get; set; }
    public required string Photos { get; set; }
    public required Company Company { get; set; }

    public Device ToEntity()
    {
        return new Device
        {
            Name = Name,
            ModelNumber = ModelNumber,
            Description = Description,
            Photos = Photos,
            Company = Company
        };
    }
}
