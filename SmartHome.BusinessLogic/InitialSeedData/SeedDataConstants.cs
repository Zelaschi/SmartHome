using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.InitialSeedData;
public static class SeedDataConstants
{
    public static Guid ADMIN_ROLE_ID = Guid.Parse("ffa636e8-ce76-4b52-b03e-8b3989bfd008");
    public static Guid HOME_OWNER_ROLE_ID = Guid.Parse("5725feee-327f-4147-aad9-ea28b9ff3e7b");
    public static Guid BUSINESS_OWNER_ROLE_ID = Guid.Parse("28a660d2-c86a-49a8-bbeb-587a82415771");
    public static Guid FIRST_ADMIN_ID = Guid.Parse("80e909fb-3c8a-423d-bd46-edde4f85fbe3");

    public static Guid CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID = Guid.Parse("f22942d7-9bc0-4458-a713-15c9010deaa1");
    public static Guid CREATE_BUSINESS_OWNER_ACCOUNT_PERMISSION_ID = Guid.Parse("b3eef741-8d56-4263-a633-7e176981feec");
    public static Guid LIST_ALL_ACCOUNTS_PERMISSION_ID = Guid.Parse("b6f31ea3-aca9-4757-9905-eff4ef100564");
    public static Guid LIST_ALL_BUSINESSES_PERMISSION_ID = Guid.Parse("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd");
    public static Guid CREATE_BUSINESS_PERMISSION_ID = Guid.Parse("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95");
    public static Guid CREATE_DEVICE_PERMISSION_ID = Guid.Parse("7c1d3527-e47c-43ac-b979-447a05558f25");
    public static Guid LIST_ALL_DEVICES_PERMISSION_ID = Guid.Parse("bf35d7ed-f4b9-410e-a427-d139ce74cf73");
    public static Guid LIST_ALL_DEVICES_TYPES_PERMISSION_ID = Guid.Parse("17133752-d60e-4f35-916f-6651ab4463e4");
    public static Guid CREATE_HOME_PERMISSION_ID = Guid.Parse("206ad2b6-e911-4491-84e4-0a6082f5f360");
    public static Guid ADD_MEMBER_TO_HOME_PERMISSION_ID = Guid.Parse("7306a4ce-47fc-4ba8-8aac-60243701cd5b");

    public static Guid ADD_MEMBER_TO_HOME_HOMEPERMISSION_ID = Guid.Parse("98bb8133-688a-4f1d-8587-87c485df6534");
    public static Guid ADD_DEVICES_TO_HOME_HOMEPERMISSION_ID = Guid.Parse("c49f2858-72fc-422d-bd4b-f49b482f80bd");
    public static Guid LIST_DEVICES_HOMEPERMISSION_ID = Guid.Parse("fa0cad23-153b-46b5-a690-91d0d7677c31");
    public static Guid RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID = Guid.Parse("9d7f6847-e8d5-4515-b9ac-0f0c00fcc7b3");
}
