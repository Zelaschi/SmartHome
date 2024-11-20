using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.DTOs;
public class DTONotification
{
    public required Notification Notification { get; set; }
    public required bool Read { get; set; }
}
