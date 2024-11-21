namespace SmartHome.ImporterCommon;
public interface IDeviceImporter
{
    public List<DTODevice> ImportDevicesFromFilePath(string path);
}
