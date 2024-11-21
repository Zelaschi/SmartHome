using SmartHome.BusinessLogic.DTOs;

namespace SmartHome.BusinessLogic.Interfaces;
public interface ILoginLogic
{
    DTOSessionAndSystemPermissions LogIn(string email, string password);
}
