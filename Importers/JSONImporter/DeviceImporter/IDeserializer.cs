namespace JSONImporter.DeviceImporter;
public interface IDeserializer
{
    public T Deserialize<T>(string path);
}
