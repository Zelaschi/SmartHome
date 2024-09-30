using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class DeviceService : IDeviceLogic
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

    public IEnumerable<Device> GetAllDevices()
    {
        throw new NotImplementedException();
    }
}
