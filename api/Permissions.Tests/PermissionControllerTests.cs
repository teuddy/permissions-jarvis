using System.Diagnostics.CodeAnalysis;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Permissions.Api.Controllers;
using Permissions.Api.Handlers.Commands;
using Permissions.Api.Handlers.Queries;
using Permissions.Api.Validation;
using Permissions.Domain.Dto;
using Permissions.Domain.Models;

namespace Permissions.Tests;

[ExcludeFromCodeCoverage]
public class PermissionControllerTests
{
    [Fact]
    public async Task GivenPermissionRequest_WhenPermissionRequestIsValid_ThenPermissionIsCreated()
    {
        // Arrange
        var permissionRequest = new PermissionRequest
        {
            EmployeeForename = "John",
            EmployeeSurname = "Smith",
            PermissionType = 1
        };
        
        var theMediator = new Mock<IMediator>();
        theMediator.Setup( mediator => mediator.Send(It.IsAny<RegisterPermissionRequestCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Permission
            {
                Id = 1,
                EmployeeForename = "John",
                EmployeeSurname = "Smith",
                PermissionTypeId = 1,
                GrantedOn = DateTime.Now
            });
        
        var permissionController = new PermissionController(theMediator.Object);

        // Act
        var result = await permissionController.RequestPermission(permissionRequest);

        // Assert
        var okResult = Assert.IsType<CreatedResult>(result);
        Assert.NotNull(result);
        
        var permission = Assert.IsType<Permission>(okResult.Value);
        Assert.Equal(1, permission.Id);
        Assert.Equal("John", permission.EmployeeForename);
        Assert.Equal("Smith", permission.EmployeeSurname);
        Assert.Equal(1, permission.PermissionTypeId);
    }
    
    
    [Fact]
    public async Task GiveAValidCall_WhenGetAllPermissionsIsCalled_ThenAllPermissionsAreReturned()
    {
        // Arrange
        var theMediator = new Mock<IMediator>();
        theMediator.Setup( mediator => 
                mediator.Send(It.IsAny<GetAllPermissionsQuery>(), 
                    It.IsAny<CancellationToken>())
                )
            .ReturnsAsync(new List<PermissionResponse>
            {
                new()
                {
                    Id = 1,
                    EmployeeForename = "John",
                    EmployeeSurname = "Smith",
                    PermissionType = new PermissionTypeResponse
                    {
                        Id = 1,
                        Description = "Annual Leave"
                    },
                    GrantedOn = DateTime.Now
                },
                new()
                {
                    Id = 2,
                    EmployeeForename = "Jane",
                    EmployeeSurname = "Doe",
                    PermissionType = new PermissionTypeResponse
                    {
                        Id = 1,
                        Description = "Annual Leave"
                    },
                    GrantedOn = DateTime.Now
                }
            });
        
        var permissionController = new PermissionController(theMediator.Object);

        // Act
        var result = await permissionController.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(result);
        
        var permissions = Assert.IsType<List<PermissionResponse>>(okResult.Value);
        Assert.Equal(2, permissions.Count);
    }
    
    [Fact]
    public async Task GivenAValidId_WhenCallGetPermissionById_ThenPermissionIsReturned()
    {
        // Arrange
        var theMediator = new Mock<IMediator>();
        theMediator.Setup( mediator => mediator.Send(It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PermissionResponse
            {
                Id = 1,
                EmployeeForename = "John",
                EmployeeSurname = "Smith",
                PermissionType = new PermissionTypeResponse
                {
                    Id = 1,
                    Description = "Annual Leave"
                },
                GrantedOn = DateTime.Now
            });
        
        var permissionController = new PermissionController(theMediator.Object);

        // Act
        var result = await permissionController.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(result);
        
        var permission = Assert.IsType<PermissionResponse>(okResult.Value);
        Assert.Equal(1, permission.Id);
        Assert.Equal("John", permission.EmployeeForename);
        Assert.Equal("Smith", permission.EmployeeSurname);
        Assert.Equal(1, permission.PermissionType.Id);
        Assert.Equal("Annual Leave", permission.PermissionType.Description);
    }

    [Fact]
    public async Task GivenAnInvalidId_WhenCallGetPermissionById_ThenNotFoundResultIsReturned()
    {
        // Arrange
        var theMediator = new Mock<IMediator>();
        theMediator.Setup( mediator => mediator.Send(It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(PermissionResponse));
        
        var permissionController = new PermissionController(theMediator.Object);

        // Act
        var result = await permissionController.Get(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.NotNull(result);
    }
    

    [Fact]
    public async Task GivenAValidIdAndPayload_WhenCallUpdatePermission_ThenNoContentResultIsReturned()
    {
        // Arrange
        var permissionRequest = new PermissionRequest
        {
            EmployeeForename = "John",
            EmployeeSurname = "Smith",
            PermissionType = 1
        };
        
        var theMediator = new Mock<IMediator>();
        theMediator.Setup( mediator => mediator.Send(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()));
        
        var permissionController = new PermissionController(theMediator.Object);

        // Act
        var result = await permissionController.Patch(1, permissionRequest);

        // Assert
        var okResult = Assert.IsType<NoContentResult>(result);
        Assert.NotNull(result);
    }
}