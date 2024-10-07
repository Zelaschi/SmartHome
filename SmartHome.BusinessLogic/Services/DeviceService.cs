﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class DeviceService : IDeviceLogic, ISecurityCameraLogic
{
    private readonly IGenericRepository<Device> _deviceRepository;
    private readonly IDeviceTypeRepository _deviceTypeRepository;
    public DeviceService(IGenericRepository<Device> deviceRepository, IDeviceTypeRepository deviceTypeRepository)
    {
        _deviceRepository = deviceRepository;
        _deviceTypeRepository = deviceTypeRepository;
    }

    public Device CreateDevice(Device device)
    {
        if (RepeatedModelNumber(device))
        {
            throw new DeviceException("Device model already exists");
        }

        return _deviceRepository.Add(device);
    }

    private bool RepeatedModelNumber(Device device)
    {
        var reapeatedDevice = _deviceRepository.Find(d => d.ModelNumber == device.ModelNumber);
        if (reapeatedDevice != null) return true;
        return false;
    }

    public SecurityCamera CreateSecurityCamera(SecurityCamera securityCamera)
    {
        if (RepeatedModelNumber(securityCamera))
        {
            throw new DeviceException("Security Camera model already exists");
        }

        return _deviceRepository.Add(securityCamera) as SecurityCamera;
    }

    public IEnumerable<Device> GetAllDevices()
    {
        var allDevices = _deviceRepository.FindAll().ToList();

        if (allDevices.Count == 0)
        {
            throw new DeviceException("There are no devices in the database.");
        }

        return allDevices;
    }

    public IEnumerable<string> GetAllDeviceTypes()
    {
        return _deviceTypeRepository.GetAllDeviceTypes();
    }
}
