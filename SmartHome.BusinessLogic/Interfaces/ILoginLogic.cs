using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.DTOs;

namespace SmartHome.BusinessLogic.Interfaces;
public interface ILoginLogic
{
    DTOSessionAndSystemPermissions LogIn(string email, string password);
}
