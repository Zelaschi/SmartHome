using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.InitialSeedData;
public static class SeedDataConstants
{
    public const string ADMIN_ROLE_ID = "ffa636e8-ce76-4b52-b03e-8b3989bfd008";
    public const string HOME_OWNER_ROLE_ID = "5725feee-327f-4147-aad9-ea28b9ff3e7b";
    public const string BUSINESS_OWNER_ROLE_ID = "28a660d2-c86a-49a8-bbeb-587a82415771";
    public const string FIRST_ADMIN_ID = "80e909fb-3c8a-423d-bd46-edde4f85fbe3";
    public const string ADMIN_HOME_OWNER_ROLE_ID = "f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4";
    public const string BUSINESS_OWNER_HOME_OWNER_ROLE_ID = "c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3";

    public const string CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID = "f22942d7-9bc0-4458-a713-15c9010deaa1";
    public const string CREATE_BUSINESS_OWNER_ACCOUNT_PERMISSION_ID = "b3eef741-8d56-4263-a633-7e176981feec";
    public const string LIST_ALL_ACCOUNTS_PERMISSION_ID = "b6f31ea3-aca9-4757-9905-eff4ef100564";
    public const string LIST_ALL_BUSINESSES_PERMISSION_ID = "4e2851c0-c5bb-4e52-b6a5-badadbbd83dd";
    public const string CREATE_BUSINESS_PERMISSION_ID = "f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95";
    public const string CREATE_DEVICE_PERMISSION_ID = "7c1d3527-e47c-43ac-b979-447a05558f25";
    public const string LIST_ALL_DEVICES_PERMISSION_ID = "bf35d7ed-f4b9-410e-a427-d139ce74cf73";
    public const string LIST_ALL_DEVICES_TYPES_PERMISSION_ID = "17133752-d60e-4f35-916f-6651ab4463e4";
    public const string CREATE_HOME_PERMISSION_ID = "206ad2b6-e911-4491-84e4-0a6082f5f360";
    public const string HOME_RELATED_PERMISSION_ID = "48644f45-4d83-4961-924b-7733881258e9";
    public const string LIST_ALL_USERS_HOMES_PERMISSION_ID = "0957ea49-6c58-4f9a-9cbc-bb4a300077f3";
    public const string LIST_ALL_USERS_NOTIFICATIONS_PERMISSION_ID = "dde0bac9-e646-4d9f-96f5-c77e7295cb4b";
    public const string CREATE_NOTIFICATION_PERMISSION_ID = "b404659a-35a4-4486-867f-db4c24f9f827";
    public const string ADD_MEMBER_TO_HOME_PERMISSION_ID = "7306a4ce-47fc-4ba8-8aac-60243701cd5b";
    public const string CREATE_ROOM_PERMISSION_ID = "f3b3b3b3-3c8a-423d-bd46-edde4f85fbe3";
    public const string ADD_PERMISSIONS_TO_HOMEMEMBER_ID = "f3b3b3b3-3c8a-423d-bd46-edde4f85fce4";

    public const string ADD_MEMBER_TO_HOME_HOMEPERMISSION_ID = "98bb8133-688a-4f1d-8587-87c485df6534";
    public const string ADD_DEVICES_TO_HOME_HOMEPERMISSION_ID = "c49f2858-72fc-422d-bd4b-f49b482f80bd";
    public const string LIST_DEVICES_HOMEPERMISSION_ID = "fa0cad23-153b-46b5-a690-91d0d7677c31";
    public const string RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID = "9d7f6847-e8d5-4515-b9ac-0f0c00fcc7b3";
}
