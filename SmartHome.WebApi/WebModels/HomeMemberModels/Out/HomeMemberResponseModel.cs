using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeMemberModels.Out;

public sealed class HomeMemberResponseModel
{
    public Guid? HomeMemberId { get; set; }
    public List<HomePermission>? HomePermissions { get; set; }
    public List<Notification>? Notifications { get; set; }
    public HomeMemberResponseModel(HomeMember homeMember)
    {
        HomeMemberId = homeMember.HomeMemberId;
        HomePermissions = homeMember.HomePermissions;
        Notifications = homeMember.Notifications;
    }

    public override bool Equals(object? obj)
    {
        return obj is HomeMemberResponseModel h && h.HomeMemberId == HomeMemberId;
    }
}
