﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Test;

[TestClass]
public class BusinessServiceTest
{
    private Mock<IGenericRepository<Business>>? businessRepositoryMock;
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private BusinessService? businessService;

    [TestInitialize]

    public void Initialize()
    {
        businessRepositoryMock = new Mock<IGenericRepository<Business>>(MockBehavior.Strict);
        userRepositoryMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        businessService = new BusinessService(businessRepositoryMock.Object, userRepositoryMock.Object);
    }

    [TestMethod]

    public void Create_Business_Test()
    {
        var businessOwner = new Role { Name = "BusinessOwner" };
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
        };
        var owner = new User
        {
            Id = Guid.NewGuid(),
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedrorodriguez@gmail.com",
            Role = businessOwner,
            Complete = false
        };

        businessRepositoryMock.Setup(x => x.Add(business)).Returns(business);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);

        userRepositoryMock.Setup(x => x.Update(It.Is<User>(u => u.Id == owner.Id))).Returns(owner);

        var businessResult = businessService.CreateBusiness(business, owner);

        businessRepositoryMock.Verify(x => x.Add(business), Times.Once);
        userRepositoryMock.Verify(x => x.Update(It.Is<User>(u => u.Id == owner.Id && u.Complete == true)), Times.Once);

        Assert.AreEqual(business, businessResult);
        Assert.IsTrue(owner.Complete);
        Assert.AreEqual(business.BusinessOwner, owner);
    }

    [TestMethod]

    public void Create_Business_With_Complete_Account_Throws_Exception_Test()
    {
        var businessOwner = new Role { Name = "BusinessOwner" };
        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
        };
        var owner = new User
        {
            Id = Guid.NewGuid(),
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pedrorodriguez@gmail.com",
            Role = businessOwner,
            Complete = true,
        };

        try
        {
            businessService.CreateBusiness(business, owner);
        }
        catch (Exception e)
        {
            businessRepositoryMock.VerifyAll();
            Assert.IsInstanceOfType(e, typeof(UserException));
            Assert.AreEqual("User is already owner of a business", e.Message);
        }
    }

    [TestMethod]
    public void GetBusinesses_WithoutFilters_ReturnsAllBusinesses()
    {
        var businesses = new List<Business>
        {
            new Business
            {
                Name = "Business1",
                BusinessOwner = new User
                                {
                                    Name = "Juan",
                                    Surname = "Perez",
                                    Password = "Password@1234",
                                    Email = "mail@mail.com"
                                },
                Id = Guid.NewGuid(),
                Logo = "Logo1",
                RUT = "1234"
            },
            new Business
            {
                Name = "Business2",
                BusinessOwner = new User
                                {
                                    Name = "Pedro",
                                    Surname = "Rodriguez",
                                    Password = "Password@1234",
                                    Email = "mail@mail.com"
                                },
                Id = Guid.NewGuid(),
                Logo = "Logo1",
                RUT = "1234"
            },
        };

        businessRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>())).Returns(businesses);

        var result = businessService.GetBusinesses(null, null, null, null);

        businessRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>()), Times.Once);
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetBusinesses_WithBusinessNameFilter_ReturnsFilteredBusinesses()
    {
        var businesses = new List<Business>
        {
            new Business
            {
                Name = "HikVision",
                BusinessOwner = new User
                                    {
                                        Name = "Juan",
                                        Surname = "Perez",
                                        Password = "Password@1234",
                                        Email = "mail@mail.com"
                                    },
                Id = Guid.NewGuid(),
                Logo = "Logo1",
                RUT = "1234"
            }
        };

        businessRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>())).Returns(businesses);

        var result = businessService.GetBusinesses(null, null, "HikVision", null);

        businessRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>()), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetBusinesses_WithFullNameFilter_ReturnsFilteredBusinesses()
    {
        var businesses = new List<Business>
        {
            new Business
            {
                Name = "Business1",
                BusinessOwner = new User
                                {
                                    Name = "Juan",
                                    Surname = "Perez",
                                    Password = "Password@1234",
                                    Email = "mail@mail.com"
                                },
                Id = Guid.NewGuid(),
                Logo = "Logo1",
                RUT = "1234"
            }
        };

        businessRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>())).Returns(businesses);

        var result = businessService.GetBusinesses(null, null, null, "Juan Perez");

        businessRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>()), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetBusinesses_WithFiltersAndPagination_ReturnsPagedFilteredBusinesses()
    {
        var businesses = new List<Business>
        {
            new Business
            {
                Name = "HikVision",
                BusinessOwner = new User
                                {
                                    Name = "Juan",
                                    Surname = "Perez",
                                    Password = "Password@1234",
                                    Email = "mail@mail.com"
                                },
                Id = Guid.NewGuid(),
                Logo = "Logo1",
                RUT = "1234"
            }
        };

        businessRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>(), 1, 1)).Returns(businesses);

        var result = businessService.GetBusinesses(1, 1, "HikVision", "Juan Perez");

        businessRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>(), 1, 1), Times.Once);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetBusinesses_WithPaginationOnly_ReturnsPagedBusinesses()
    {
        var businesses = new List<Business>
        {
            new Business
            {
                Name = "Business1",
                BusinessOwner = new User
                                {
                                    Name = "Juan",
                                    Surname = "Perez",
                                    Password = "Password@1234",
                                    Email = "mail@mail.com"
                                },
                Id = Guid.NewGuid(),
                Logo = "Logo1",
                RUT = "1234"
            }
        };

        businessRepositoryMock.Setup(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>(), 1, 1)).Returns(businesses);

        var result = businessService.GetBusinesses(1, 1, null, null);

        businessRepositoryMock.Verify(x => x.FindAllFiltered(It.IsAny<Expression<Func<Business, bool>>>(), 1, 1), Times.Once);
        Assert.AreEqual(1, result.Count());
    }
}
