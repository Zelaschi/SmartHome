using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IAuthorizationLogic
{
    public Guid GetUserIdByToken(Guid token);
    public string GetUserRoleByToken(Guid guid);
}
