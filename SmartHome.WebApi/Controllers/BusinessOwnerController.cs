using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.WebApi.Controllers;

[Route("api/v1/businessOwners")]
[ApiController]
public class BusinessOwnerController
{
    private readonly IBusinessOwnerLogic _businessOwnerLogic;
    public BusinessOwnerController(IBusinessOwnerLogic businessOwnerLogic) 
    {
        _businessOwnerLogic = businessOwnerLogic;
    }
}
