using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain;
public class User
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public bool? Complete { get; set; }
    public string? ProfilePhoto { get; set; }
    public List<Home> Houses { get; set; }
    public required Role Role { get; set; }
    public required DateTime CreationDate = DateTime.Today;

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
