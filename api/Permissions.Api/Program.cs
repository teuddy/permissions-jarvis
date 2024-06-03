using Mediator;
using Permissions.Api.Middleware;
using Permissions.Api.Validation;
using Permissions.Infrastructure;
using Permissions.Infrastructure.Data;
using Permissions.Infrastructure.Elastic;
using Permissions.Infrastructure.Event;

var builder = WebApplication.CreateBuilder(args);

// Get Configuration
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDatabase(configuration);
builder.Services.AddEventQueue(configuration);
builder.Services.AddElasticsearchConfig(configuration);
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Transient;
});

builder.Services.AddCors(options => 
    options.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        })
    );

builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(MessageValidatorBehaviour<,>));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

app.CrateDataBaseIfNotExists();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.Run();

public partial class Program {}