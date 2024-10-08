using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IHomePermissionLogic
{
    public bool HasPermission(Guid userId, Guid homeId, Guid permissionId);
}
