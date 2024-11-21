using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IUsersLogic
{
    IEnumerable<User> GetUsers(int? pageNumber, int? pageSize, string? role, string? fullName);
}
