﻿using System.Reflection;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.DeviceImporter.TypeMap;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.ImporterCommon;

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
            case JSONTypeMap.InteligentLamp:
                device.Type = DeviceTypesStatic.InteligentLamp;
                break;
            case JSONTypeMap.WindowSensor:
                device.Type = DeviceTypesStatic.WindowSensor;
                break;
            case JSONTypeMap.MovementSensor:
                device.Type = DeviceTypesStatic.MovementSensor;
                break;
        }
    }

    private Type GetImporterType(string dllName)
    {
        Type returnedType = null;
        var dllFiles = Directory.GetFiles(_importerPath, "*.dll");

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
                        returnedType = type;
                    }

                    break;
                }
            }
        }

        if (returnedType == null)
        {
            throw new DeviceImporterException("DLL was not found!");
        }

        return returnedType;
    }

    private int RegisterDevicesInDatabaseFromDTODevies(List<DTODevice> dtodevices, string dllName, User user)
    {
        var addedDevices = dtodevices.Count;
        for (var i = 0; i < dtodevices.Count; i++)
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
                catch (DeviceException e)
                {
                    addedDevices--;
                    Console.WriteLine("Coudnt register device " + camera.ModelNumber + " Error: " + e.Message);
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
                catch (DeviceException e)
                {
                    addedDevices--;
                    Console.WriteLine("Coudnt register device " + device.ModelNumber + " Error: " + e.Message);
                }
            }
        }

        return addedDevices;
    }

    public int ImportDevices(string dllName, string fileName, User user)
    {
        Type importerType = GetImporterType(dllName);

        var importer = (IDeviceImporter)Activator.CreateInstance(importerType);
        var inputFileFullPath = _devicesFilesPath + "\\" + fileName;
        List<DTODevice> dtodevices;
        try
        {
            dtodevices = importer.ImportDevicesFromFilePath(inputFileFullPath);
        }
        catch (Exception)
        {
            throw new DeviceImporterException("Could not load devices from file. Please check file name.");
        }

        var addedDevices = RegisterDevicesInDatabaseFromDTODevies(dtodevices, dllName, user);

        return addedDevices;
    }
}
