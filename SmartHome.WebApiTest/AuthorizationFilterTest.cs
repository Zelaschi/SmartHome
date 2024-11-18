using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SmartHome.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc.Abstractions;
using SmartHome.WebApi.Filters;
using Microsoft.AspNetCore.Routing;

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
