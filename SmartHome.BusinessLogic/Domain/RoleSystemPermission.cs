using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public class RoleSystemPermission
{
    public int RoleId { get; set; }
    public required Role Role { get; set; }

    public int SystemPermissionId { get; set; }
    public required RoleSystemPermission SystemPermission { get; set; }
}
