using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class HomeMember
{
    public Guid HomeMemberId { get; set; }
    public List<HomePermission> HomePermissions { get; set; }
    public List<Notification> Notifications { get; set; }
    public User User { get; set; }
    public bool IsOwner { get; set; } = false;

    public HomeMember(User user, bool isOwner)
    {
        HomeMemberId = Guid.NewGuid();
        HomePermissions = new List<HomePermission>();
        Notifications = new List<Notification>();
        User = user;
        IsOwner = isOwner;
    }
}
