namespace SmartHome.BusinessLogic.Domain;
public class HomeMemberPermission
{
    public Guid HomeMemberId { get; set; }
    public required HomeMember HomeMember { get; set; }

    public Guid PermissionId { get; set; }
    public required HomePermission Permission { get; set; }
}
