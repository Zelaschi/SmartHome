using Newtonsoft.Json;
using Importers.DeviceImporter;

namespace Importers.JSON;
internal class JSONDeserializer : IDeserializer
{
    public T Deserialize<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json);
    }
}
