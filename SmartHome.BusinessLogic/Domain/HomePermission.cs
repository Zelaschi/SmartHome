using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class HomePermission
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<HomeMember> HomeMembers { get; set; }
    public HomePermission()
    {
        HomeMembers = new List<HomeMember>();
    }
}
