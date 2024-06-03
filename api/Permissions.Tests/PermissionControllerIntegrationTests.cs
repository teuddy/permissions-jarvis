using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Permissions.Domain.Dto;
using Permissions.Domain.Models;
using Permissions.Infrastructure.DataAccess;

namespace Permissions.Tests;

[ExcludeFromCodeCoverage]
public class PermissionControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public PermissionControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetPermissionById_ShouldReturnPermission_WhenPermissionExists()
    {
        // Arrange
        var permission = new Permission
        {
            EmployeeForename = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            GrantedOn = DateTime.UtcNow
        };

        var permissionType = new PermissionType
        {
            Description = "Annual Leave"
        };
        
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PermissionsContext>();
        await context.Database.EnsureCreatedAsync();
        await context.PermissionTypes.AddAsync(permissionType);
        await context.SaveChangesAsync();
        await context.Permissions.AddAsync(permission);
        await context.SaveChangesAsync();

        var client = _factory.CreateClient();
        
        // Act
        
        var response = await client.GetAsync($"/v1/permission/1");
        var responseContent = await response.Content.ReadAsStringAsync();

        var responsePermission = JsonSerializer.Deserialize<PermissionResponse>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        // Assert

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responsePermission);
        Assert.Equal(permission.Id, responsePermission.Id);
        Assert.Equal(permission.EmployeeForename, responsePermission.EmployeeForename);
        Assert.Equal(permission.EmployeeSurname, responsePermission.EmployeeSurname);
        Assert.Equal(permission.PermissionType.Id, responsePermission.PermissionType.Id);
        Assert.Equal(permission.PermissionType.Description, responsePermission.PermissionType.Description);
        Assert.Equal(permission.GrantedOn, responsePermission.GrantedOn);
        
        // Clean up
        await context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task GetPermissionById_ShouldReturnNotFound_WhenPermissionDoesNotExist()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PermissionsContext>();
        await context.Database.EnsureCreatedAsync();
        
        // Act
        
        var response = await client.GetAsync($"/v1/permission/1");

        // Assert

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        // Clean up
        await context.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async Task GetAllPermissions_ShouldReturnAListOfPermissions_WhenPermissionsExist()
    {
        // Arrange
        var permission = new Permission
        {
            EmployeeForename = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            GrantedOn = DateTime.UtcNow
        };

        var permissionType = new PermissionType
        {
            Description = "Annual Leave"
        };
        
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PermissionsContext>();
        await context.Database.EnsureCreatedAsync();
        await context.PermissionTypes.AddAsync(permissionType);
        await context.SaveChangesAsync();
        await context.Permissions.AddAsync(permission);
        await context.SaveChangesAsync();

        var client = _factory.CreateClient();
        
        // Act
        
        var response = await client.GetAsync($"/v1/permission");
        var responseContent = await response.Content.ReadAsStringAsync();

        var responsePermissions = JsonSerializer.Deserialize<List<PermissionResponse>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        // Assert

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responsePermissions);
        Assert.NotEmpty(responsePermissions);
        Assert.Equal(permission.Id, responsePermissions.First().Id);
        Assert.Equal(permission.EmployeeForename, responsePermissions.First().EmployeeForename);
        Assert.Equal(permission.EmployeeSurname, responsePermissions.First().EmployeeSurname);
        Assert.Equal(permission.PermissionType.Id, responsePermissions.First().PermissionType.Id);
        Assert.Equal(permission.PermissionType.Description, responsePermissions.First().PermissionType.Description);
        Assert.Equal(permission.GrantedOn, responsePermissions.First().GrantedOn);
        
        // Clean up
        await context.Database.EnsureDeletedAsync();
    }
}