using System.Linq.Expressions;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class DeviceService : IDeviceLogic, ISecurityCameraLogic, ICreateDeviceLogic
{
    private readonly IGenericRepository<Device> _deviceRepository;
    private readonly IGenericRepository<Business> _businessRepository;
    private readonly ValidatorService _validatorService;
    public DeviceService(IGenericRepository<Business> businessRepository,
                        IGenericRepository<Device> deviceRepository,
                        ValidatorService validatorService)
    {
        _businessRepository = businessRepository;
        _deviceRepository = deviceRepository;
        _validatorService = validatorService;
    }

    public SecurityCamera CreateSecurityCamera(SecurityCamera securityCamera, User bOwner)
    {
        var business = _businessRepository.Find(x => x.BusinessOwner == bOwner);
        if (business == null)
        {
            throw new DeviceException("Business was not found for the user");
        }

        if ((business.ValidatorId != null) && (!_validatorService.IsValidModelNumber(securityCamera.ModelNumber, business.ValidatorId)))
        {
            throw new DeviceException("Model number is not valid");
        }

        securityCamera.Business = business;
        return _deviceRepository.Add(securityCamera) as SecurityCamera;
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

        if ((business.ValidatorId != null) && (!_validatorService.IsValidModelNumber(device.ModelNumber, business.ValidatorId)))
        {
            throw new DeviceException("Model number is not valid");
        }

        if (_deviceRepository.Find(x => x.Id == device.Id) != null)
        {
            throw new DeviceException("There is already a device with the id " + device.Id + " in the database");
        }

        device.Business = business;
        device.Type = type;
        return _deviceRepository.Add(device);
    }

    public IEnumerable<Device> GetDevices(int? pageNumber, int? pageSize, string? deviceName, string? deviceModel, string? businessName, string? deviceType)
    {
        Expression<Func<Device, bool>> filter = device =>
            (string.IsNullOrEmpty(deviceName) || device.Name == deviceName) &&
            (string.IsNullOrEmpty(deviceModel) || device.ModelNumber == deviceModel) &&
            (string.IsNullOrEmpty(businessName) || device.Business.Name == businessName) &&
            (string.IsNullOrEmpty(deviceType) || device.Type == deviceType);

        if (pageNumber == null && pageSize == null)
        {
            return _deviceRepository.FindAllFiltered(filter);
        }

        return _deviceRepository.FindAllFiltered(filter, pageNumber ?? 1, pageSize ?? 10);
    }
}
