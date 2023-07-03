using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TimeWorkedManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerResponse(401, "Unauthorized")]
public class ApiControllerBase : Controller
{
    
}