using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
public interface IHomesFromUserRepository
{
    IEnumerable<Home> GetAllHomesByUserId(Guid userId);
}
