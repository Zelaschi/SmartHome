using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;

public interface IAdminLogic
{
    User CreateAdmin(User user);
    void DeleteAdmin(Guid adminId);
}
