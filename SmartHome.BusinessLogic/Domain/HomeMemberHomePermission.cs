using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
namespace SmartHome.BusinessLogic.Domain;
public class HomeMemberHomePermission
{
    public int HomeMemberId { get; set; }
    public required HomeMember HomeMember { get; set; }

    public int PermissionId { get; set; }
    public required HomeMemberPermission Permission { get; set; }
}
