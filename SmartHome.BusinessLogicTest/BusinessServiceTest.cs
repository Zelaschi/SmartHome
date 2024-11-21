using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Services;
using System.Linq.Expressions;

namespace SmartHome.BusinessLogic.Test;

[TestClass]
public class BusinessServiceTest
{
    private Mock<IGenericRepository<Business>>? businessRepositoryMock;
    private Mock<IGenericRepository<User>>? userRepositoryMock;
    private Mock<IGenericRepository<ModelNumberValidator>>? validatorRepositoryMock;
    private ValidatorService? validatorService;
    private BusinessService? businessService;

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
        businessRepositoryMock = new Mock<IGenericRepository<Business>>(MockBehavior.Strict);
        userRepositoryMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        validatorRepositoryMock = new Mock<IGenericRepository<ModelNumberValidator>>();
        validatorService = new ValidatorService(validatorRepositoryMock.Object);
        businessService = new BusinessService(businessRepositoryMock.Object, userRepositoryMock.Object, validatorRepositoryMock.Object, validatorService);
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
    public void AddValidatorToBusiness_BusinessNotFound_ThrowsException()
    {
        var businessOwner = new User
        {
            Id = Guid.NewGuid(),
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pr@mail.com"
        };

        Business business = null;

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>())).Returns(business);

        Exception exception = null;

        try
        {
            businessService.AddValidatorToBusiness(businessOwner, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        businessRepositoryMock.VerifyAll();

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(BusinessException));
        Assert.AreEqual("Business does not exist", exception.Message);
    }

    [TestMethod]
    public void GetAllValidators_ShouldReturnExistingValidators_WhenFoundInRepository()
    {
        var validatorNames = new List<string> { "LetterNumberModelValidator", "SixLettersModelValidator" };

        var existingValidators = new List<ModelNumberValidator>
    {
        new ModelNumberValidator
        {
            Id = Guid.Parse("a10f1893-4454-4588-afbe-0f0eb9332c3c"),
            Name = "LetterNumberModelValidator"
        },
        new ModelNumberValidator
        {
            Id = Guid.Parse("c46e1484-3838-4f61-ac33-d4708f5e3a5b"),
            Name = "SixLettersModelValidator"
        }
    };

        var addedValidators = new List<ModelNumberValidator>();

        validatorRepositoryMock
            .Setup(v => v.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
            .Returns<Func<ModelNumberValidator, bool>>(filter =>
                existingValidators.FirstOrDefault(filter));

        validatorRepositoryMock
            .Setup(v => v.Add(It.IsAny<ModelNumberValidator>()))
            .Callback<ModelNumberValidator>(validator => addedValidators.Add(validator));

        var result = businessService.GetAllValidators();

        result.Should().HaveCount(2);
        result.First(v => v.Name == "LetterNumberModelValidator").ValidatorId.Should().Be(Guid.Parse("a10f1893-4454-4588-afbe-0f0eb9332c3c"));
        result.First(v => v.Name == "SixLettersModelValidator").ValidatorId.Should().Be(Guid.Parse("c46e1484-3838-4f61-ac33-d4708f5e3a5b"));

        addedValidators.Should().BeEmpty();
    }

    [TestMethod]
    public void GetAllValidators_ShouldReturnTwo_WhenNoValidatorsAdded()
    {
        validatorRepositoryMock.Setup(v => v.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
            .Returns((ModelNumberValidator)null);
        validatorService = new ValidatorService(validatorRepositoryMock.Object);
        businessService = new BusinessService(
            businessRepositoryMock.Object,
            userRepositoryMock.Object,
            validatorRepositoryMock.Object,
            validatorService);

        var result = businessService.GetAllValidators();

        result.Should().HaveCount(2);
    }

    [TestMethod]
    public void GetBusinessByUser_BusinessNotFound_ThrowsException()
    {
        var user = new User
        {
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pr@mail.com"
        };

        Business business = null;

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>()))
                    .Returns(business);

        Exception exception = null;

        try
        {
            businessService.GetBusinessByUser(user);
        }
        catch (Exception e)
        {
            exception = e;
        }

        businessRepositoryMock.VerifyAll();

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(BusinessException));
        Assert.AreEqual("Business does not exist", exception.Message);
    }

    [TestMethod]
    public void GetBusinessByUser_BusinessFound_ReturnsBusiness()
    {
        var businessOwner = new User
        {
            Id = Guid.NewGuid(),
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pr@mail.com"
        };

        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
            BusinessOwner = businessOwner,
            ValidatorId = Guid.NewGuid()
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>()))
                    .Returns(business);

        var result = businessService.GetBusinessByUser(businessOwner);

        businessRepositoryMock.VerifyAll();
        Assert.IsNotNull(result);
        Assert.AreEqual(business, result);
    }

    [TestMethod]
    public void GetBusinesses_WithPagination_ReturnsPagedBusinesses()
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
        new Business
        {
            Name = "Business3",
            BusinessOwner = new User
                            {
                                Name = "Luis",
                                Surname = "Martinez",
                                Password = "Password@1234",
                                Email = "mail@mail.com"
                            },
            Id = Guid.NewGuid(),
            Logo = "Logo1",
            RUT = "1234"
        }
    };

        businessRepositoryMock.Setup(x => x.FindAllFiltered(
                It.IsAny<Expression<Func<Business, bool>>>(),
                It.Is<int>(p => p == 2), // Página 2
                It.Is<int>(s => s == 1)) // Tamaño 1 (un registro por página)
            )
            .Returns(businesses.Skip(1).Take(1).ToList());

        var result = businessService.GetBusinesses(2, 1, null, null);

        businessRepositoryMock.Verify(
            x => x.FindAllFiltered(
                It.IsAny<Expression<Func<Business, bool>>>(),
                It.Is<int>(p => p == 2),
                It.Is<int>(s => s == 1)),
            Times.Once);

        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Business2");
    }

    [TestMethod]
    public void AddValidatorToBusiness_BusinessAlreadyHasValidator_ShouldThrowException()
    {
        var businessOwner = new User
        {
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pr@mail.com"
        };

        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
            BusinessOwner = businessOwner,
            ValidatorId = Guid.NewGuid()
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>()))
                    .Returns(business);

        Exception exception = null;

        try
        {
            businessService.AddValidatorToBusiness(businessOwner, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        businessRepositoryMock.VerifyAll();

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(BusinessException));
        Assert.AreEqual("Business already has a validator", exception.Message);
    }

    [TestMethod]
    public void AddValidatorToBusiness_ValidatorNotFoundInDB_ShouldThrowException()
    {
        var businessOwner = new User
        {
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pr@mail.com"
        };

        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
            BusinessOwner = businessOwner,
            ValidatorId = null
        };

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>()))
                    .Returns(business);

        Exception exception = null;

        try
        {
            businessService.AddValidatorToBusiness(businessOwner, Guid.NewGuid());
        }
        catch (Exception e)
        {
            exception = e;
        }

        businessRepositoryMock.VerifyAll();

        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(ValidatorException));
        Assert.AreEqual("Validator was not found in database", exception.Message);
    }

    [TestMethod]
    public void AddValidatorToBusiness_AddsCorrectly()
    {
        var businessOwner = new User
        {
            Name = "Pedro",
            Surname = "Rodriguez",
            Password = "Password@1234",
            CreationDate = DateTime.Today,
            Email = "pr@mail.com"
        };

        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = "HikVision",
            Logo = "Logo1",
            RUT = "1234",
            BusinessOwner = businessOwner,
            ValidatorId = null
        };

        var validator = new ModelNumberValidator
        {
            Id = Guid.Parse("c46e1484-3838-4f61-ac33-d4708f5e3a5b"),
            Name = "SixLettersModelValidator"
        };

        validatorRepositoryMock.Setup(u => u.Find(It.IsAny<Func<ModelNumberValidator, bool>>()))
            .Returns(validator);

        businessRepositoryMock.Setup(u => u.Find(It.IsAny<Func<Business, bool>>()))
            .Returns(business);

        businessRepositoryMock.Setup(u => u.Update(It.Is<Business>(b => b.Id == business.Id)))
            .Returns(business);

        var businessFound = businessService.AddValidatorToBusiness(businessOwner, validator.Id);

        businessRepositoryMock.VerifyAll();
        validatorRepositoryMock.VerifyAll();

        business.ValidatorId.Should().Be(validator.Id);
        Assert.AreEqual(businessFound, business);
    }
}
