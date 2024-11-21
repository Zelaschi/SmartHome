namespace SmartHome.BusinessLogic.Domain;
public class RoleSystemPermission
{
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }

    public Guid SystemPermissionId { get; set; }
    public SystemPermission? SystemPermission { get; set; }
}
