using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.DeviceImporter;
using SmartHome.BusinessLogic.DeviceTypes;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class DeviceImportService : IDeviceImportLogic
{
    private readonly string _importerPath = @"..\DeviceImporter\ImporterDLLs";
    private readonly string _devicesFilesPath = @"..\DeviceImporter\DevicesFiles";
    private readonly ICreateDeviceLogic _createDeviceLogic;

    public DeviceImportService(ICreateDeviceLogic createDeviceLogic)
    {
        _createDeviceLogic = createDeviceLogic;
    }

    public List<Device> ImportDevices(string dllName, string fileName, User user)
    {
        var dllFiles = Directory.GetFiles(_importerPath, "*.dll");

        Type importadorType = null;
        foreach (var dllFile in dllFiles)
        {
            var assembly = Assembly.LoadFrom(dllFile);
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetInterface("IDeviceImporter") != null && type.Name == dllName)
                {
                    importadorType = type;
                }
            }
        }

        if (importadorType == null)
        {
            throw new Exception("placeholder");
        }

        var importer = (IDeviceImporter)Activator.CreateInstance(importadorType);
        var inputFullPath = _devicesFilesPath + "\\" + fileName;

        List<DTODevice> dtodevices = importer.ImportDevicesFromFilePath(inputFullPath);

        var devices = new List<Device>();

        foreach (var dtodevice in dtodevices)
        {
            var photos = new List<Photo>();
            foreach (var photo in dtodevice.Photos)
            {
                photos.Add(new Photo { Path = photo.Path });
            }

            if (dtodevice.Type.Equals(DeviceTypesStatic.SecurityCamera))
            {
                var camera = new SecurityCamera
                {
                    Id = dtodevice.Id,
                    Name = dtodevice.Name,
                    ModelNumber = dtodevice.Model,
                    Description = string.Empty,
                    Photos = photos,
                    PersonDetection = dtodevice.PersonDetection ?? false,
                    MovementDetection = dtodevice.MovementDetection ?? false
                };
                devices.Add(_createDeviceLogic.CreateDevice(camera, user, camera.Type));
            }
            else
            {
                var device = new Device
                {
                    Id = dtodevice.Id,
                    Name = dtodevice.Name,
                    ModelNumber = dtodevice.Model,
                    Description = string.Empty,
                    Photos = photos
                };

                devices.Add(_createDeviceLogic.CreateDevice(device, user, device.Type));
            }
        }

        return devices;
    }
}
