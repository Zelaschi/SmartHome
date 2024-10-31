using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.DeviceTypes;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.ImporterCommon;
using SmartHome.BusinessLogic.DeviceImporter.TypeMap;
using SmartHome.BusinessLogic.CustomExceptions;

namespace SmartHome.BusinessLogic.Services;
public sealed class DeviceImportService : IDeviceImportLogic
{
    private readonly string _importerPath = @"..\SmartHome.BusinessLogic\DeviceImporter\ImporterDLLs";
    private readonly string _devicesFilesPath = @"..\SmartHome.BusinessLogic\DeviceImporter\DevicesFiles";
    private readonly ICreateDeviceLogic _createDeviceLogic;

    public DeviceImportService(ICreateDeviceLogic createDeviceLogic)
    {
        _createDeviceLogic = createDeviceLogic;
    }

    private void JSONDeviceTypeMapper(ref DTODevice device)
    {
        switch (device.Type)
        {
            case JSONTypeMap.SecurityCamera:
                device.Type = DeviceTypesStatic.SecurityCamera;
                break;
            case JSONTypeMap.IntelligentLamp:
                device.Type = DeviceTypesStatic.IntelligentLamp;
                break;
            case JSONTypeMap.WindowSensor:
                device.Type = DeviceTypesStatic.WindowSensor;
                break;
            case JSONTypeMap.MovementSensor:
                device.Type = DeviceTypesStatic.MovementSensor;
                break;
        }
    }

    public int ImportDevices(string dllName, string fileName, User user)
    {
        var dllFiles = Directory.GetFiles(_importerPath, "*.dll");

        Type importadorType = null;
        foreach (var dllFile in dllFiles)
        {
            var assembly = Assembly.LoadFrom(dllFile);
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetInterface("IDeviceImporter") != null)
                {
                    var importerInstance = Activator.CreateInstance(type);
                    var dllNameValue = type.GetField("DllName").GetValue(importerInstance);
                    if (dllNameValue.Equals(dllName))
                    {
                        importadorType = type;
                    }

                    break;
                }
            }
        }

        if (importadorType == null)
        {
            throw new Exception("placeholder");
        }

        var importer = (IDeviceImporter)Activator.CreateInstance(importadorType);
        var inputFileFullPath = _devicesFilesPath + "\\" + fileName;

        List<DTODevice> dtodevices = importer.ImportDevicesFromFilePath(inputFileFullPath);

        var addedDevices = dtodevices.Count;

        for (var i= 0; i< dtodevices.Count; i++)
        {
            var dtodevice = dtodevices[i];
            var photos = new List<Photo>();
            foreach (var photo in dtodevice.Photos)
            {
                photos.Add(new Photo { Path = photo.Path });
            }

            switch (dllName)
            {
                case "JSON":
                    JSONDeviceTypeMapper(ref dtodevice);
                    break;
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
                try
                {
                    _createDeviceLogic.CreateDevice(camera, user, dtodevice.Type);
                }
                catch (DeviceException)
                {
                    addedDevices--;
                }
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
                try
                {
                    _createDeviceLogic.CreateDevice(device, user, dtodevice.Type);
                }
                catch (DeviceException)
                {
                    addedDevices--;
                }
            }
        }

        return addedDevices;
    }
}
