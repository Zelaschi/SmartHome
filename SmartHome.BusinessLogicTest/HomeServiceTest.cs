using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    private HomeService? homeService;

    [TestInitialize]
    public void Initialize()
    {
        homeMemberRepositoryMock = new Mock<IGenericRepository<HomeMember>>(MockBehavior.Strict);
        homeRepositoryMock = new Mock<IGenericRepository<Home>>(MockBehavior.Strict);
        homeService = new HomeService(homeRepositoryMock.Object, homeMemberRepositoryMock.Object);
    }
}
