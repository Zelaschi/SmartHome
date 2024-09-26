using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;

public interface IAdminLogic
{
    User CreateAdmin(User user);
    void DeleteAdmin(Guid adminId);
}
