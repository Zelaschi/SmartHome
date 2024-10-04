using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public class RoleService : IRoleLogic, ISystemPermissionLogic
{
    private readonly IGenericRepository<Role> _roleRepository;

    public RoleService(IGenericRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public virtual Role GetHomeOwnerRole()
    {
        return _roleRepository.Find(role => role.Id == Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID)) ?? throw new RoleException("Role not found");
    }

    public virtual Role GetBusinessOwnerRole()
    {
        return _roleRepository.Find(role => role.Id == Guid.Parse(SeedDataConstants.BUSINESS_OWNER_ROLE_ID)) ?? throw new RoleException("Role not found");
    }

    public virtual Role GetAdminRole()
    {
        return _roleRepository.Find(role => role.Id == Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID)) ?? throw new RoleException("Role not found");
    }

    public bool HasPermission(Guid roleId, Guid permissionId)
    {
        throw new NotImplementedException();
    }

    public SystemPermission GetSystemPermissionById(Guid systemPermissionId)
    {
        throw new NotImplementedException();
    }
}
