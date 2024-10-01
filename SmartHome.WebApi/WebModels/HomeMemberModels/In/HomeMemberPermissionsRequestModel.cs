using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeMemberModels.In;

public class HomeMemberPermissions
{
    public bool AddMemberPermission { get; set; }
    public bool AddDevicePermission { get; set; }
    public bool ListDevicesPermission { get; set; }
    public bool NotificationsPermission { get; set; }

    public List<HomePermission> ToHomePermissionList()
    {
        var homePermissions = new List<HomePermission>();
        if (AddMemberPermission)
            homePermissions.Add(new HomePermission { Name = "AddMemberPermission" });
        if (AddDevicePermission)
            homePermissions.Add(new HomePermission { Name = "AddDevicesPermission" });
        if (ListDevicesPermission)
            homePermissions.Add(new HomePermission { Name = "ListDevicesPermission" });
        if (NotificationsPermission)
            homePermissions.Add(new HomePermission { Name = "NotificationsPermission" });

        return homePermissions;
    }
}
