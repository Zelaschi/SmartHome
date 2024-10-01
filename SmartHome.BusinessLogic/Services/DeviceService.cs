using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class DeviceService : IDeviceLogic, ISecurityCameraLogic
{
    private readonly IGenericRepository<Device> _deviceRepository;
    public DeviceService(IGenericRepository<Device> deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public Device CreateDevice(Device device)
    {
        throw new NotImplementedException();
    }

    public SecurityCamera CreateSecurityCamera(SecurityCamera securityCamera)
    {
        return _deviceRepository.Add(securityCamera) as SecurityCamera;
    }

    public IEnumerable<Device> GetAllDevices()
    {
        return _deviceRepository.FindAll();
    }
}
