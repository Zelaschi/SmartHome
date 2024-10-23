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
    public List<string>? HomePermissions { get; set; }
    public List<string>? Notifications { get; set; }
    public HomeMemberResponseModel(HomeMember homeMember)
    {
        HomeMemberId = homeMember.HomeMemberId;
        foreach (var homePermission in homeMember.HomePermissions)
        {
            HomePermissions.Add(homePermission.Name);
        }

        foreach (var notification in homeMember.Notifications)
        {
            Notifications.Add(notification.ToString());
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is HomeMemberResponseModel h && h.HomeMemberId == HomeMemberId;
    }
}
