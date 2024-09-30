namespace SmartHome.WebApi.WebModels.HomeMemberModels.In;

public class HomeMemberPermissions
{
    public bool AddMemberPermission { get; set; }
    public bool AddDevicePermission { get; set; }
    public bool ListDevicesPermission { get; set; }
    public bool NotificationsPermission { get; set; }
}
