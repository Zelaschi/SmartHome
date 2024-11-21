using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IRoleLogic
{
    public Role GetHomeOwnerRole();
    public Role GetBusinessOwnerRole();
    public Role GetAdminRole();
    public Role GetAdminHomeOwnerRole();
    public Role GetBusinessOwnerHomeOwnerRole();
    public bool HasPermission(Guid roleId, Guid permissionId);
}
