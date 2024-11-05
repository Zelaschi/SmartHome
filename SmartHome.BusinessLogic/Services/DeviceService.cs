using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.DeviceTypes;

namespace SmartHome.BusinessLogic.Services;
public sealed class DeviceService : IDeviceLogic, ISecurityCameraLogic, ICreateDeviceLogic
{
    private readonly IGenericRepository<Device> _deviceRepository;
    private readonly IGenericRepository<Business> _businessRepository;
    public DeviceService(IGenericRepository<Business> businessRepository, IGenericRepository<Device> deviceRepository)
    {
        _businessRepository = businessRepository;
        _deviceRepository = deviceRepository;
    }

    private bool RepeatedModelNumber(Device device)
    {
        var reapeatedDevice = _deviceRepository.Find(d => d.ModelNumber == device.ModelNumber);
        if (reapeatedDevice != null) return true;
        return false;
    }

    public SecurityCamera CreateSecurityCamera(SecurityCamera securityCamera, User bOwner)
    {
        var business = _businessRepository.Find(x => x.BusinessOwner == bOwner);
        if (business == null)
        {
            throw new DeviceException("Business was not found for the user");
        }

        if (RepeatedModelNumber(securityCamera))
        {
            throw new DeviceException("Security Camera model already exists");
        }

        securityCamera.Business = business;
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
        return new List<string>
        {
            DeviceTypesStatic.SecurityCamera,
            DeviceTypesStatic.InteligentLamp,
            DeviceTypesStatic.WindowSensor,
            DeviceTypesStatic.MovementSensor
        };
    }

    public Device CreateDevice(Device device, User user, string type)
    {
        var business = _businessRepository.Find(x => x.BusinessOwner == user);
        if (business == null)
        {
            throw new DeviceException("Business was not found for the user");
        }

        if (RepeatedModelNumber(device))
        {
            throw new DeviceException("Device model already exists");
        }

        device.Business = business;
        device.Type = type;
        return _deviceRepository.Add(device);
    }
}
