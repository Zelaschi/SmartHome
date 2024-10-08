using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IRoleLogic
{
    public Role GetHomeOwnerRole();
    public Role GetBusinessOwnerRole();
    public Role GetAdminRole();
    public bool HasPermission(Guid roleId, Guid permissionId);
}
