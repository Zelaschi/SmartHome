﻿using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Services;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.CustomExceptions;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Test;

[TestClass]
public class RoleRepositoryTest
{
    private readonly SmartHomeEFCoreContext _context;
    private readonly RoleRepository _roleRepository;

    public RoleRepositoryTest()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _roleRepository = new RoleRepository(_context);
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
    [TestMethod]
    public void AddRole_WhenRoleIsAdded_ShouldReturnRole()
    {
        _context.Roles.RemoveRange(_context.Roles);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        using var otherContext = DbContextBuilder.BuildTestDbContext();
        var rolesSaved = otherContext.Roles.ToList();

        rolesSaved.Count.Should().Be(1);
        var roleSaved = rolesSaved[0];
        roleSaved.Id.Should().Be(role.Id);
        roleSaved.Name.Should().Be(role.Name);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenRoleExists_ShouldRemoveFromDatabase()
    {
        _context.Roles.RemoveRange(_context.Roles);
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        _roleRepository.Delete(role.Id);

        _context.Roles.FirstOrDefault(r => r.Id == role.Id).Should().BeNull();
    }
    #endregion

    #region Find
    #endregion

    #region Update
    #endregion

    #region GetAll
    #endregion
}
