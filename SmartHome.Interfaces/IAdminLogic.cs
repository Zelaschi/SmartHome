using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Domain;

namespace SmartHome.Interfaces;

public interface IAdminLogic
{
    User CreateAdmin(User user);
    IEnumerable<User> GetAllUsers();
    void DeleteAdmin(User admin);
    User CreateBusinessOwner(User bOwner);
    IEnumerable<Company> GetAllCompanies();
}
