using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain;
public class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public bool Complete { get; set; }
    public string ProfilePhoto { get; set; }
    public List<Home> Houses { get; set; }
    public Role Role { get; set; }

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
