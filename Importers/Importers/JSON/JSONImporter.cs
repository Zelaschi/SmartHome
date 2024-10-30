using Importers.IncomingData;
using Importers.DeviceImporter;

namespace Importers.JSON;
internal class JSONImporter : IDeviceImporter
{
    public List<DTODevice> ImportDevicesFromFilePath(string path)
    {
        IDeserializer _jsonDeserializer = new JSONDeserializer();
        Root deviceData = _jsonDeserializer.Deserialize<Root>(path);
        var devices = new List<DTODevice>();
        MapDispositivosToDTODevice(deviceData, devices);

        return devices;
    }

    private void MapDispositivosToDTODevice(Root deviceData, List<DTODevice> devices)
    {
        foreach (var dispositivo in deviceData.Dispositivos)
        {
            var device = new DTODevice
            {
                Id = dispositivo.Id,
                Type = dispositivo.Tipo,
                Name = dispositivo.Nombre,
                Model = dispositivo.Modelo,
                Photos = new List<DTOPhoto>(),
                PersonDetection = dispositivo.PersonDetection,
                MovementDetection = dispositivo.MovementDetection
            };
            ConvertFotosToDTOPhotos(dispositivo, device);

            devices.Add(device);
        }
    }

    private void ConvertFotosToDTOPhotos(Dispositivo dispositivo, DTODevice device)
    {
        foreach (var foto in dispositivo.Fotos)
        {
            var photo = new DTOPhoto
            {
                Path = foto.Path,
                IsPrincipal = foto.EsPrincipal
            };

            device.Photos.Add(photo);
        }
    }
}
