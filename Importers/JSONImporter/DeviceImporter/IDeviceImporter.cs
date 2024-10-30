namespace JSONImporter.DeviceImporter;
public interface IDeviceImporter
{
    public List<DTODevice> ImportDevicesFromFilePath(string path);
}
