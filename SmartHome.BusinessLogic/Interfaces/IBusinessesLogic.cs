using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.DTOs;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IBusinessesLogic
{
    Business CreateBusiness(Business business, User user);
    IEnumerable<Business> GetBusinesses(int? pageNumber, int? pageSize, string? businessName, string? fullName);
    List<DTOValidator> GetAllValidators();
    Business AddValidatorToBusiness(User user, Guid validatorId);
}
