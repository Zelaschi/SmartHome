namespace SmartHome.BusinessLogic.Interfaces;
public interface IHomePermissionLogic
{
    public bool HasPermission(Guid userId, Guid homeId, Guid permissionId);
}
