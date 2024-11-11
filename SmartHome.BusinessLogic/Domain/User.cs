using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class User
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public bool? Complete { get; set; }
    public string? ProfilePhoto { get; set; }
    public DateTime CreationDate { get; set; }

    [JsonIgnore]
    public List<Home>? Houses { get; set; }
    public Role? Role { get; set; }
    public Guid RoleId { get; set; }

    public User()
    {
        Houses = new List<Home>();
    }

    public void AssignRole(Role role)
    {
        Role = role;
    }

    public bool HasPermission(SystemPermission permission)
    {
        return Role.HasPermission(permission);
    }
}
