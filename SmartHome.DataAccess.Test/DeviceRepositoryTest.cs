﻿using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class DeviceRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly DeviceRepository _deviceRepository;

    public DeviceRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _deviceRepository = new DeviceRepository(_context);
    }

    [TestInitialize]
    public void Setup()
    {
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add
    #region Success
    [TestMethod]
    public void Add_WhenInfoIsProvided_ShouldAddedToDatabase()
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = "Test Device",
            ModelNumber = "Test Model Number",
            Description = "Test Description",
            Photos = new List<Photo>()
        };

        _deviceRepository.Add(device);

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var devicesSaved = otherContext.Devices.ToList();

        devicesSaved.Count.Should().Be(1);
        var deviceSaved = devicesSaved[0];
        deviceSaved.Id.Should().Be(device.Id);
        deviceSaved.Name.Should().Be(device.Name);
    }

    #endregion
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        var device = new Device
        {
            Description = "Test Description",
            Id = Guid.NewGuid(),
            ModelNumber = "Test Model Number",
            Name = "Test Device",
            Photos = new List<Photo>()
        };
        _context.Devices.Add(device);
        _context.SaveChanges();

        var devices = _deviceRepository.FindAll();

        devices.Count.Should().Be(1);
        devices[0].Id.Should().Be(device.Id);
        devices[0].Name.Should().Be(device.Name);
    }
    #endregion
}
