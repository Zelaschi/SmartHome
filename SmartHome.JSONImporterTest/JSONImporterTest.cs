using SmartHome.JSONImporter;
using SmartHome.JSONImporter.IncomingData;

namespace SmartHome.JSONImporterTest;

[TestClass]
public class JSONImporterTest
{
    private JsonImporterClass? jsonImporter;
    public required string path;

    [TestInitialize]
    public void TestInitialize()
    {
        jsonImporter = new JsonImporterClass() { DllName = "JSON" };
        path = @"..\..\..\devices-to-import.json";
    }

    [TestMethod]
    public void ImportDevicesFromFileTest()
    {
        var dispositivos = new List<Dispositivo>()
        {
            new Dispositivo()
            {
                Id = Guid.Parse("69508433-1569-47a4-9591-447c3c4bdcbd"),
                Tipo = "camera",
                Nombre = "Business G235",
                Modelo = "G235",
                Person_Detection = false,
                Movement_Detection = true,
                Fotos = new List<Foto>()
                {
                    new Foto()
                    {
                        Path = "https://http2.mlstatic.com/D_NQ_NP_2X_787853-MLU71131483530_082023-F.webp",
                        EsPrincipal = false
                    },
                    new Foto()
                    {
                        Path = "https://http2.mlstatic.com/D_NQ_NP_2X_603102-MLU71171145159_082023-F.webp",
                        EsPrincipal = true
                    }
                }
            },
            new Dispositivo()
            {
                Id = Guid.Parse("cc077ab4-432b-43b9-85d3-d256dcc887fb"),
                Tipo = "sensor-open-close",
                Nombre = "Kasa A540",
                Modelo = "A540",
                Fotos = new List<Foto>()
                {
                    new Foto()
                    {
                        Path = "https://http2.mlstatic.com/D_NQ_NP_2X_737708-MLU75995534252_052024-F.webp",
                        EsPrincipal = false
                    },
                    new Foto()
                    {
                        Path = "https://http2.mlstatic.com/D_NQ_NP_2X_859473-MLU78851895417_082024-F.webp",
                        EsPrincipal = true
                    }
                }
            }
        };

        var root = new Root()
        {
            Dispositivos = dispositivos
        };

        var devices = jsonImporter.ImportDevicesFromFilePath(path);

        Assert.AreEqual(root.Dispositivos.Count, devices.Count);
        Assert.AreEqual(root.Dispositivos[0].Nombre, devices[0].Name);
    }
}
