using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IBusinessesLogic
{
    Business CreateBusiness(Business business, User user);
    IEnumerable<Business> GetBusinesses(int? pageNumber, int? pageSize, string? businessName, string? fullName);
    List<string> GetAllValidators();
    Business AddValidatorToBusiness(Guid businessId, Guid validatorId);
}
