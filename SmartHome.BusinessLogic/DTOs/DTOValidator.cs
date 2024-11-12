using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.DTOs;
public sealed class DTOValidator
{
    public Guid ValidatorId { get; set; }
    public required string Name { get; set; }
}
