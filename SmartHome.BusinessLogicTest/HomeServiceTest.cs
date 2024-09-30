﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogicTest;

[TestClass]
public class HomeServiceTest
{
    private Mock<IGenericRepository<Home>>? homeRepositoryMock;
    private Mock<IGenericRepository<HomeMember>>? homeMemberRepositoryMock;
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private HomeService? homeService;
    private Role? homeOwnerRole;

    [TestInitialize]
    public void Initialize()
    {
        homeMemberRepositoryMock = new Mock<IGenericRepository<HomeMember>>(MockBehavior.Strict);
        homeRepositoryMock = new Mock<IGenericRepository<Home>>(MockBehavior.Strict);
        userRepositoryMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        homeService = new HomeService(homeRepositoryMock.Object, homeMemberRepositoryMock.Object, userRepositoryMock.Object);
        homeOwnerRole = new Role { Name = "HomeOwner" };
    }

    [TestMethod]
    public void Create_Home_OK_Test()
    {
        var ownerId = Guid.NewGuid();
        var owner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = ownerId, Role = homeOwnerRole };
        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };

        homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);

        var result = homeService.CreateHome(home, ownerId);

        userRepositoryMock.VerifyAll();
        homeRepositoryMock.VerifyAll();
        Assert.AreEqual(home, result);
    }

    [TestMethod]
    public void Register_HomeMemberToHouse_OK_Test()
    {
        var ownerId = Guid.NewGuid();
        var owner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = ownerId, Role = homeOwnerRole };
        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        var memberId = Guid.NewGuid();
        var member = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = memberId, Role = homeOwnerRole };

        homeRepositoryMock.Setup(x => x.Update(It.IsAny<Home>())).Returns(home);
        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);
        userRepositoryMock.Setup(x => x.Find(It.IsAny<Func<User, bool>>())).Returns(owner);

        homeService.AddHomeMemberToHome(home.Id, memberId);
        homeRepositoryMock.VerifyAll();
        userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetAll_HomeMembers_Test()
    {
        var owner = new User { Email = "blankEmail@blank.com", Name = "blankName", Surname = "blanckSurname", Password = "blankPassword", Id = new Guid(), Role = homeOwnerRole };
        var member1 = new User { Email = "blankEmail1@blank.com", Name = "blankName1", Surname = "blanckSurname1", Password = "blankPassword", Id = new Guid(), Role = homeOwnerRole };
        var member2 = new User { Email = "blankEmail2@blank.com", Name = "blankName2", Surname = "blanckSurname2", Password = "blankPassword", Id = new Guid(), Role = homeOwnerRole };
        var homeOwner = new HomeMember(owner, true);
        var homeMember1 = new HomeMember(member1, false);
        var homeMember2 = new HomeMember(member2, false);

        var home = new Home { Id = Guid.NewGuid(), MainStreet = "Street", DoorNumber = "123", Latitude = "-31", Longitude = "31", MaxMembers = 6, Owner = owner };
        home.Members.Add(homeOwner);
        home.Members.Add(homeMember1);
        home.Members.Add(homeMember2);

        homeRepositoryMock.Setup(x => x.Find(It.IsAny<Func<Home, bool>>())).Returns(home);

        homeService.GetAllHomeMembers(home.Id);
        homeRepositoryMock.VerifyAll();
    }
}
