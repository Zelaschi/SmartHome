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
}
