using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class BusinessService : IBusinessesLogic
{
    private readonly IGenericRepository<Business> _businessRepository;

    public BusinessService(IGenericRepository<Business> businessRepository)
    {
        _businessRepository = businessRepository;
    }

    public Business CreateBusiness(Business business)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Business> GetAllBusinesses()
    {
        return _businessRepository.FindAll();
    }
}
