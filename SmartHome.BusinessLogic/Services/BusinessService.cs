using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ModeloValidador.Abstracciones;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class BusinessService : IBusinessesLogic
{
    private readonly IGenericRepository<Business> _businessRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Validator> _validatorRespository;
    private readonly ValidatorService _validatorService;

    public BusinessService(IGenericRepository<Business> businessRepository,
                            IGenericRepository<User> userRepository,
                            IGenericRepository<Validator> validatorRespository,
                            ValidatorService validatorService)
    {
        _businessRepository = businessRepository;
        _userRepository = userRepository;
        _validatorRespository = validatorRespository;
        _validatorService = validatorService;
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

    public IEnumerable<Business> GetBusinesses(int? pageNumber, int? pageSize, string? businessName, string? fullName)
    {
        Expression<Func<Business, bool>> filter = business =>
            (string.IsNullOrEmpty(businessName) || business.Name == businessName) &&
            (string.IsNullOrEmpty(fullName) ||
                (business.BusinessOwner.Name.ToLower() + " " + business.BusinessOwner.Surname.ToLower()).Contains(fullName.ToLower()) ||
                (business.BusinessOwner.Surname.ToLower() + " " + business.BusinessOwner.Name.ToLower()).Contains(fullName.ToLower()));

        if (pageNumber == null && pageSize == null)
        {
            return _businessRepository.FindAllFiltered(filter);
        }

        return _businessRepository.FindAllFiltered(filter, pageNumber ?? 1, pageSize ?? 10);
    }

    public List<string> GetAllValidators()
    {
        return _validatorService.GetAllValidators();
    }

    public Business AddValidatorToBusiness(Guid businessId, Guid validatorId)
    {
        var business = _businessRepository.Find(b => b.Id == businessId);
        var validatorName = _validatorRespository.Find(v => v.Id == validatorId).Name;
        var validator = _validatorService.GetImplementation(validatorId.ToString());

        if (business == null)
        {
            throw new BusinessException("Business does not exist");
        }

        if (validator == null)
        {
            throw new ValidatorException("Validator does not exist");
        }

        business.ValidatorId = validatorId;
        _businessRepository.Update(business);
        return business;
    }
}
