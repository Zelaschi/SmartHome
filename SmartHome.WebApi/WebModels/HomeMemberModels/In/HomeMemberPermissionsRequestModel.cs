using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeMemberModels.In;

public class HomeMemberPermissions
{
    public bool AddMemberPermission { get; set; }
    public bool AddDevicePermission { get; set; }
    public bool ListDevicesPermission { get; set; }
    public bool NotificationsPermission { get; set; }

    public List<HomeMemberPermission> ToHomePermissionList()
    {
        var homePermissions = new List<HomeMemberPermission>();
        if (AddMemberPermission)
            homePermissions.Add(new HomeMemberPermission { Name = "AddMemberPermission" });
        if (AddDevicePermission)
            homePermissions.Add(new HomeMemberPermission { Name = "AddDevicesPermission" });
        if (ListDevicesPermission)
            homePermissions.Add(new HomeMemberPermission { Name = "ListDevicesPermission" });
        if (NotificationsPermission)
            homePermissions.Add(new HomeMemberPermission { Name = "NotificationsPermission" });

        return homePermissions;
    }
}
