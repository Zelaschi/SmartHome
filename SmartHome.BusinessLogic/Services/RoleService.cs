using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.InitialSeedData;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public class RoleService : IRoleLogic, ISystemPermissionLogic
{
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IGenericRepository<SystemPermission> _systemPermissionRepository;

    public RoleService(IGenericRepository<Role> roleRepository, IGenericRepository<SystemPermission> systemPermissionRepository)
    {
        _roleRepository = roleRepository;
        _systemPermissionRepository = systemPermissionRepository;
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

    public virtual Role GetAdminHomeOwnerRole()
    {
        return _roleRepository.Find(role => role.Id == Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID)) ?? throw new RoleException("Role not found");
    }

    public virtual Role GetBusinessOwnerHomeOwnerRole()
    {
        return _roleRepository.Find(role => role.Id == Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID)) ?? throw new RoleException("Role not found");
    }

    public bool HasPermission(Guid roleId, Guid permissionId)
    {
        var role = _roleRepository.Find(x => x.Id == roleId);
        if (role == null)
        {
            throw new RoleException("Role not found");
        }

        var permission = role.SystemPermissions.FirstOrDefault(x => x.Id == permissionId);
        if (permission != null)
        {
            return true;
        }

        return false;
    }

    public SystemPermission GetSystemPermissionById(Guid systemPermissionId)
    {
        return _systemPermissionRepository.Find(permission => permission.Id == systemPermissionId) ?? throw new RoleException("Permission not found");
    }
}
