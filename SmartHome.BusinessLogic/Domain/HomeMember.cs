﻿using System;
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
    public List<HomeMemberNotification> HomeMemberNotifications { get; set; }
    public User? User { get; set; }
    public Guid HomeId { get; set; }
    public HomeMember()
    {
        User = User;
        HomeMemberId = Guid.NewGuid();
        HomePermissions = new List<HomePermission>();
        Notifications = new List<Notification>();
        HomeMemberNotifications = new List<HomeMemberNotification>();
    }

    public HomeMember(User? user)
        : this()
    {
        User = user;
    }
}
