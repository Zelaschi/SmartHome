using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IUsersLogic
{
    IEnumerable<User> GetAllUsers();
}
