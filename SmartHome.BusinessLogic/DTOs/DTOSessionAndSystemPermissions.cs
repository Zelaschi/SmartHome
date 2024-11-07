using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.DTOs;
public class DTOSessionAndSystemPermissions
{
    public Guid SessionId { get; set; }
    public required List<SystemPermission> SystemPermissions { get; set; }
}
