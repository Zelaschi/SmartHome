using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface ISystemPermissionLogic
{
    public SystemPermission GetSystemPermissionById(Guid systemPermissionId);
}
