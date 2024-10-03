using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IRoleLogic
{
    public Role GetHomeOwnerRole();
    public Role GetBusinessOwnerRole();
    public Role GetAdminRole();
    public bool HasPermission(Guid roleId, Guid permissionId);
}
