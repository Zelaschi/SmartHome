using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.WebModels.BusinessOwnerModels.In;

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

    public CreatedAtActionResult CreateBusinessOwner(BusinessOwnerRequestModel businessOwnerRequestModel)
    {
        throw new NotImplementedException();
    }
}
