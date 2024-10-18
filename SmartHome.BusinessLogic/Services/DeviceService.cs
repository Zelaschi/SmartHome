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

namespace SmartHome.BusinessLogic.Services;
public sealed class DeviceService : IDeviceLogic, ISecurityCameraLogic, IWindowSensorLogic, IMovementSensorLogic, IInteligentLampLogic
{
    private readonly IGenericRepository<Device> _deviceRepository;
    private readonly IDeviceTypeRepository _deviceTypeRepository;
    private readonly IGenericRepository<Business> _businessRepository;
    public DeviceService(IGenericRepository<Business> businessRepository, IGenericRepository<Device> deviceRepository, IDeviceTypeRepository deviceTypeRepository)
    {
        _businessRepository = businessRepository;
        _deviceRepository = deviceRepository;
        _deviceTypeRepository = deviceTypeRepository;
    }

    public WindowSensor CreateWindowSensor(WindowSensor device, User user)
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

        return _deviceRepository.Add(device) as WindowSensor;
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
        return _deviceTypeRepository.GetAllDeviceTypes();
    }

    public Device CreateMovementSensor(Device device, User user)
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

        return _deviceRepository.Add(device);
    }

    public InteligentLamp CreateInteligentLamp(InteligentLamp device, User user)
    {
        var business = _businessRepository.Find(x => x.BusinessOwner == user);
        if (business == null)
        {
            throw new DeviceException("Business was not found for the user");
        }

        if (RepeatedModelNumber(device))
        {
            throw new DeviceException("Security Camera model already exists");
        }

        device.Business = business;
        return _deviceRepository.Add(device) as InteligentLamp;
    }
}
