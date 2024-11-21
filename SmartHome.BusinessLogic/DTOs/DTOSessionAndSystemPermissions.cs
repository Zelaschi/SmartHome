using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.DTOs;
public class DTOSessionAndSystemPermissions
{
    public Guid SessionId { get; set; }
    public required List<SystemPermission> SystemPermissions { get; set; }
}
