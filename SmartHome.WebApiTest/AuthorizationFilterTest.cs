using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Test;

[TestClass]
public class AuthorizationFilterTests
{
    private Mock<HttpContext>? _httpContextMock;
    private readonly Mock<ISessionLogic>? _sessionServiceMock;
    private AuthorizationFilterContext? _context;
    private readonly AuthenticationFilter? _attribute;

    public AuthorizationFilterTests()
    {
        _attribute = new AuthenticationFilter();
        _sessionServiceMock = new Mock<ISessionLogic>();
    }

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_sessionServiceMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _httpContextMock.Setup(h => h.RequestServices).Returns(serviceProvider);

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>()
        );
    }
}
