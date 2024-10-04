using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface INotificationLogic
{
    List<Notification> GetNotificationsByHomeMemberId(Guid homeMemberId);

    Notification CreateMovementDetectionNotification(Guid homeDeviceId);

    Notification CreatePersonDetectionNotification(Guid homeDeviceId, Guid userid);
}
