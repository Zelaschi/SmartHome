using System.Text.Json.Serialization;

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
