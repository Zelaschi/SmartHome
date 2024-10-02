using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.HomeMemberModels.Out;

namespace SmartHome.WebApi.WebModels.HomeMemberModels.Out;

public sealed class HomeMemberResponseModel
{
    public Guid? HomeMemberId { get; set; }
    public List<HomeMemberHomePermission>? HomePermissions { get; set; }
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
