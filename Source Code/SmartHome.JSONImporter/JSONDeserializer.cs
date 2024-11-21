using Newtonsoft.Json;
namespace SmartHome.JSONImporter;
public class JSONDeserializer : IDeserializer
{
    public T Deserialize<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json);
    }
}
