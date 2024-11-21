namespace SmartHome.WebApi.WebModels.DeviceModels.Out;

public sealed class DeviceTypesResponseModel
{
    public string Type { get; set; }

    public DeviceTypesResponseModel(string type)
    {
        Type = type;
    }

    public override bool Equals(object? obj)
    {
        return obj is DeviceTypesResponseModel d && d.Type == Type;
    }
}
