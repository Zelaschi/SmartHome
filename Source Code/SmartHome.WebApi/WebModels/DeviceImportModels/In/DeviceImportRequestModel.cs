namespace SmartHome.WebApi.WebModels.DeviceImportModels.In;

public sealed class DeviceImportRequestModel
{
    public required string DllName { get; set; }
    public required string FileName { get; set; }
}
