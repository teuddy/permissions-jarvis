using Mediator;
using Microsoft.AspNetCore.Mvc;
using Permissions.Api.Handlers.Commands;
using Permissions.Api.Handlers.Queries;
using Permissions.Domain.Dto;

namespace Permissions.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class PermissionController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public PermissionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetAllPermissionsQuery());
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetPermissionByIdQuery(id));
            
        if (result == null)
        {
            return NotFound();
        }
            
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> RequestPermission([FromBody]PermissionRequest request)
    {
        var result = await _mediator.Send(new RegisterPermissionRequestCommand(request.EmployeeForename,
            request.EmployeeSurname, request.PermissionType));
        return Created($"v1/permission/{result}", result);
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromBody]PermissionRequest request)
    {
        await _mediator.Send(new UpdatePermissionCommand(id, request.EmployeeForename,
            request.EmployeeSurname, request.PermissionType));
        return NoContent();
    }
}