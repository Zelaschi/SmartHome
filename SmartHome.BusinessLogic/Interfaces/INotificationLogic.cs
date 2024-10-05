﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface INotificationLogic
{
    List<Notification> GetUsersNotifications(User user);

    Notification CreateMovementDetectionNotification(Guid homeDeviceId);

    Notification CreatePersonDetectionNotification(Guid homeDeviceId, Guid userid);

    Notification CreateOpenCloseWindowNotification(Guid homeDeviceId, bool open);
}
