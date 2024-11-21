using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IBusinessOwnerLogic
{
    User CreateBusinessOwner(User user);
    void UpdateBusinessOwnerRole(User user);
}
