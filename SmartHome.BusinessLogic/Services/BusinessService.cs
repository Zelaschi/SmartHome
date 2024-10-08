using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class BusinessService : IBusinessesLogic
{
    private readonly IGenericRepository<Business> _businessRepository;
    private readonly IGenericRepository<User> _userRepository;

    public BusinessService(IGenericRepository<Business> businessRepository, IGenericRepository<User> userRepository)
    {
        _businessRepository = businessRepository;
        _userRepository = userRepository;
    }

    private Business AssignOwnerToBusiness(Business business, User user)
    {
        business.BusinessOwner = user;
        user.Complete = true;
        _userRepository.Update(user);
        return _businessRepository.Add(business);
    }

    private bool OwnerAccountComplete(User user)
    {
        return (bool)user.Complete;
    }

    public Business CreateBusiness(Business business, User user)
    {
        if (OwnerAccountComplete(user))
        {
            throw new UserException("User is already owner of a business");
        }

        return AssignOwnerToBusiness(business, user);
    }

    public IEnumerable<Business> GetAllBusinesses()
    {
        return _businessRepository.FindAll();
    }
}
