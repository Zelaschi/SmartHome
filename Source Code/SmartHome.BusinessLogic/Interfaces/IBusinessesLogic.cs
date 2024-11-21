using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.DTOs;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IBusinessesLogic
{
    Business CreateBusiness(Business business, User user);
    IEnumerable<Business> GetBusinesses(int? pageNumber, int? pageSize, string? businessName, string? fullName);
    List<DTOValidator> GetAllValidators();
    Business AddValidatorToBusiness(User user, Guid validatorId);
    Business GetBusinessByUser(User user);
}
