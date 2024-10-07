using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;

public sealed class Role
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    [JsonIgnore]
    public List<SystemPermission> SystemPermissions { get; set; }

    public Role()
    {
        SystemPermissions = new List<SystemPermission>();
    }

    public void AddPermission(SystemPermission permission)
    {
        SystemPermissions.Add(permission);
    }

    public void RemovePermission(SystemPermission permission)
    {
        SystemPermissions.Remove(permission);
    }

    public bool HasPermission(SystemPermission permission)
    {
        return SystemPermissions.Contains(permission);
    }
}
