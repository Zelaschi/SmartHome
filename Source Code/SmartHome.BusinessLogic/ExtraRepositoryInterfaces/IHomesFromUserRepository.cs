using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
public interface IHomesFromUserRepository
{
    IEnumerable<Home> GetAllHomesByUserId(Guid userId);
}
