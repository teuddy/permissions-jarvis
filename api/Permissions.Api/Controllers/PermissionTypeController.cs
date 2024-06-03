using Mediator;
using Microsoft.AspNetCore.Mvc;
using Permissions.Api.Handlers.Commands;
using Permissions.Api.Handlers.Queries;
using Permissions.Domain.Dto;

namespace Permissions.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class PermissionTypeController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public PermissionTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetAllPermissionTypesQuery());
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]PermissionTypeRequest request)
    {
        var result = await _mediator.Send(new CreatePermissionTypeCommand(request.Description));
        return Ok(result);
    }
}