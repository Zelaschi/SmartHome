using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.DTOs;
public class DTONotification
{
    public required Notification Notification { get; set; }
    public required bool Read { get; set; }
}
