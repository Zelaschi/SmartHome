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
public class RoleService : IRoleLogic
{
    private readonly IGenericRepository<Role> _roleRepository;
    private const string HomeOwnerRole = "HomeOwner";
    private const string BusinessOwnerRole = "BusinessOwner";

    public RoleService(IGenericRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public virtual Role GetHomeOwnerRole()
    {
        return _roleRepository.Find(role => role.Name == HomeOwnerRole) ?? throw new RoleException("Role not found");
    }

    public virtual Role GetBusinessOwnerRole()
    {
        return _roleRepository.Find(role => role.Name == BusinessOwnerRole) ?? throw new RoleException("Role not found");
    }
}
