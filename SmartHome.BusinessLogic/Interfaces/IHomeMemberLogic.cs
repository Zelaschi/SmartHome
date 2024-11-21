using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IHomeMemberLogic
{
    void UpdateHomePermissionsOfHomeMember(Guid homeMemberId, List<HomePermission> permissions, Guid? homeOwnerId);
    List<HomePermission> GetAllHomePermissions();
}
