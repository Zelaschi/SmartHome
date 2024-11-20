using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.DTOs;

namespace SmartHome.BusinessLogic.Interfaces;
public interface INotificationLogic
{
    List<DTONotification> GetUsersNotifications(User user);

    Notification CreateMovementDetectionNotification(Guid homeDeviceId);

    Notification CreatePersonDetectionNotification(Guid homeDeviceId, Guid userid);

    Notification CreateOpenCloseWindowNotification(Guid homeDeviceId);
}
