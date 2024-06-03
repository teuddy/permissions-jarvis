using System.Net;
using Permissions.Api.Validation;

namespace Permissions.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation("attempting to execute request");
            await _next.Invoke(context);
        }
        catch (ValidationException e)
        {
            _logger.LogInformation("Request raised a Validation error");
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(e.Error.Errors);
        }
        catch (PermissionException e)
        {
            _logger.LogInformation("Request raised a Permission error");
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                ErrorMessage = e.Message
            });
        }
    }
}