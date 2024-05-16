using System.Text;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using IOTBackend.API.Configuration;
using IOTBackend.Persistance;
using IOTBackend.Infrastructure;
using IOTBackend.Application;
using IOTBackend.API.Controllers;
using IOTBackend.Application.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));

// inject persistant tier services
builder.Services.AddPersistanceServices(builder.Configuration);

// inject infrastruture tier services
builder.Services.AddInfrastructureServices(builder.Configuration);

// inject application tier services
builder.Services.AddApplicationServices(builder.Configuration);

// inject repository services
RepoConfigurations.ConfigureService(builder.Services, builder.Configuration);

// inject authentication middlware
builder.Services.AddAuthentication( x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("AppSettings:Token").Value!))
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();

app.Map("/ws", builder =>
{
    builder.Use(async (context, next) =>
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            // Resolve IWebSocketService from the service provider
            var webSocketService = app.Services.GetRequiredService<IWebSocketService>();

            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var controller = new WebSocketController(webSocket, webSocketService);
            await controller.HandleWebSocket();
        }
        else
        {
            await next();
        }
    });
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

Console.WriteLine(builder.Configuration.GetConnectionString("AppDbConnection"));

app.MapControllers();

app.Run();
