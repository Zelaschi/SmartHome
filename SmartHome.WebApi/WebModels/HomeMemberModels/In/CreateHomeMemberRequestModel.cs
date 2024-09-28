using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeMemberModels.In;

public sealed class HomeMemberRequestModel
{
    public required Guid HomeMemberId { get; set; }
    public required List<HomePermission> HomePermissions { get; set; }
    public required List<Notification> Notifications { get; set; }

    public HomeMember ToEntitiy()
    {
        return new HomeMember()
        {
            HomeMemberId = HomeMemberId,
            HomePermissions = HomePermissions,
            Notifications = Notifications,
        };
    }
}
