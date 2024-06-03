using System.Diagnostics.CodeAnalysis;
using Mediator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Permissions.Api.Validation;
using Permissions.Domain.Dto;
using Permissions.Domain.Events;
using Permissions.Domain.Models;
using Permissions.Domain.Repositories;
using Permissions.Infrastructure;
using Permissions.Infrastructure.DataAccess;
using Permissions.Infrastructure.DataAccess.Repository;

namespace Permissions.Tests;

[ExcludeFromCodeCoverage]
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<PermissionsContext>));
                
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }
                
            services.AddMediator(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Transient;
            });
            services.AddDbContext<PermissionsContext>(options =>
            {
                options.UseInMemoryDatabase("GetAllPermissions_ShouldReturnAllPermissions_WhenPermissionsExist");
            });
        
            services.AddControllers();
            services.AddLogging();
            services.AddTransient(typeof(IRepository<Permission>), typeof(PermissionsRepository));
            services.AddTransient(typeof(IRepository<PermissionType>), typeof(PermissionTypeRepository));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(MessageValidatorBehaviour<,>));
        
            var mockPublisher = new Mock<IPublisher<PermissionEvent>>();

            services.AddTransient(typeof(IPublisher<PermissionEvent>), _ => mockPublisher.Object);
        });
        
        builder.UseEnvironment("Development");
    }
}