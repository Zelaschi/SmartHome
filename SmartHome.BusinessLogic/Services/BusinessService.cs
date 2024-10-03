using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public Business CreateBusiness(Business business, User user)
    {
        if ((bool)user.Complete)
        {
            throw new UserException("User is already owner of a business");
        }

        business.BusinessOwner = user;
        user.Complete = true;
        _userRepository.Update(user);
        return _businessRepository.Add(business);
    }

    public IEnumerable<Business> GetAllBusinesses()
    {
        return _businessRepository.FindAll();
    }
}
