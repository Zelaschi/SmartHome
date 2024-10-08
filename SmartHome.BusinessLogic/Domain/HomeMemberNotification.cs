using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public class HomeMemberNotification
{
    public Guid NotificationId { get; set; }
    public required Notification Notification { get; set; }
    public Guid HomeMemberId { get; set; }
    public required HomeMember HomeMember { get; set; }
    public bool Read { get; set; } = false;
}
