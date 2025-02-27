﻿using Moq;
using SmartHome.BusinessLogic.Constants;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Test;

[TestClass]
public class DeviceImportServiceTest
{
    private Mock<IGenericRepository<Device>>? deviceRepositoryMock;
    private Mock<IGenericRepository<Business>>? businessRepositoryMock;
    private Mock<IGenericRepository<ModelNumberValidator>>? validatorRepositoryMock;
    private ValidatorService? validatorService;
    private DeviceService? deviceService;
    private DeviceImportService? deviceImportService;
    private readonly Role businessOwner = new Role() { Name = "businessOwner" };
    private readonly string path = @".\test.json";
    private readonly string originModelV =
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "SmartHome.BusinessLogicTest", "SmartHome.ModelValidator");
    private readonly string destinationModelV = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "SmartHome.BusinessLogicTest", "bin", "Debug", "SmartHome.ModelValidator");

    private readonly string originBL =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "SmartHome.BusinessLogicTest", "SmartHome.BusinessLogic");
    private readonly string destinationBL = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "SmartHome.BusinessLogicTest", "bin", "Debug", "SmartHome.BusinessLogic");

    [TestInitialize]

    public void Initialize()
    {
        MoveFiles(originModelV, destinationModelV);
        MoveFiles(originBL, destinationBL);
        deviceRepositoryMock = new Mock<IGenericRepository<Device>>();
        businessRepositoryMock = new Mock<IGenericRepository<Business>>();
        validatorRepositoryMock = new Mock<IGenericRepository<ModelNumberValidator>>();
        validatorService = new ValidatorService(validatorRepositoryMock.Object);
        deviceService = new DeviceService(businessRepositoryMock.Object, deviceRepositoryMock.Object, validatorService);
        deviceImportService = new DeviceImportService(deviceService);
    }

    [TestCleanup]
    public void Cleanup()
    {
        MoveFiles(destinationModelV, originModelV);
        MoveFiles(destinationBL, originBL);
    }

    private static void MoveFiles(string sourcePath, string destinationPath)
    {
        if (Directory.Exists(sourcePath))
        {
            Directory.Move(sourcePath, destinationPath);
        }
    }

    [TestMethod]
    public void ImportDevicesTest_OK()
    {
        var user1 = new User()
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Surname = "b",
            Password = "psw1",
            Email = "mail1@mail.com",
            Role = businessOwner,
            CreationDate = DateTime.Today
        };
        var company1 = new Business()
        {
            Id = Guid.NewGuid(),
            Name = "hikvision",
            Logo = "logo1",
            RUT = "rut1",
            BusinessOwner = user1
        };
        var device1 = new SecurityCamera()
        {
            Id = Guid.Parse("69508433-1569-47a4-9591-447c3c4bdcbd"),
            Name = "Business G235",
            Description = "Camera 1",
            ModelNumber = "G235",
            Type = DeviceTypesStatic.SecurityCamera,
            Business = company1,
            Photos = new List<Photo>()
        };
        var device2 = new Device
        {
            Id = Guid.Parse("cc077ab4-432b-43b9-85d3-d256dcc887fb"),
            Name = "Kasa A540",
            Description = "Window Sensor 1",
            ModelNumber = "A540",
            Business = company1,
            Photos = new List<Photo>(),
            Type = DeviceTypesStatic.WindowSensor,
        };
        var expected = new List<Device>()
        {
            device1,
            device2
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns(company1);
        deviceRepositoryMock.SetupSequence(d => d.Add(It.IsAny<Device>()))
            .Returns(device1)
            .Returns(device2);

        var result = deviceImportService.ImportDevices("JSON", path, user1);
        Assert.AreEqual(expected.Count, result);
    }
}
