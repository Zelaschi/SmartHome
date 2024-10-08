using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IBusinessesLogic
{
    Business CreateBusiness(Business business, User user);
    IEnumerable<Business> GetAllBusinesses();
}
