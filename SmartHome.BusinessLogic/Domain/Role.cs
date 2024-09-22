using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;

public class Role
{
    public required string Name { get; set; }
    public List<SystemPermission> Permissions { get; set; }

    public Role()
    {
        Permissions = new List<SystemPermission>();
    }

    public void AddPermission(SystemPermission permission)
    {
        Permissions.Add(permission);
    }

    public void RemovePermission(SystemPermission permission)
    {
        Permissions.Remove(permission);
    }

    public bool HasPermission(SystemPermission permission)
    {
        return Permissions.Contains(permission);
    }
}
