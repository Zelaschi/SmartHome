using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;

public sealed class Role
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<RoleSystemPermission> Permissions { get; set; }

    public Role()
    {
        Permissions = new List<RoleSystemPermission>();
    }

    public void AddPermission(RoleSystemPermission permission)
    {
        Permissions.Add(permission);
    }

    public void RemovePermission(RoleSystemPermission permission)
    {
        Permissions.Remove(permission);
    }

    public bool HasPermission(RoleSystemPermission permission)
    {
        return Permissions.Contains(permission);
    }
}
