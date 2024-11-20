using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;

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
            homePermissions.Add(new HomePermission() { Name = "AddMemberPermission", Id = Guid.Parse(SeedDataConstants.ADD_MEMBER_TO_HOME_HOMEPERMISSION_ID) });
        if (AddDevicePermission)
            homePermissions.Add(new HomePermission() { Name = "AddDevicesPermission" , Id = Guid.Parse(SeedDataConstants.ADD_DEVICES_TO_HOME_HOMEPERMISSION_ID)});
        if (ListDevicesPermission)
            homePermissions.Add(new HomePermission { Name = "ListDevicesPermission" , Id = Guid.Parse(SeedDataConstants.LIST_DEVICES_HOMEPERMISSION_ID)});
        if (NotificationsPermission)
            homePermissions.Add(new HomePermission { Name = "NotificationsPermission", Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID) });

        return homePermissions;
    }
}
