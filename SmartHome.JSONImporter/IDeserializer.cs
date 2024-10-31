namespace SmartHome.JSONImporter;
public interface IDeserializer
{
    public T Deserialize<T>(string path);
}
