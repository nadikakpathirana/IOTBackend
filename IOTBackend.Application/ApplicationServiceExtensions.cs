using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using IOTBackend.Application.Interfaces;
using IOTBackend.Application.Services;
using IOTBackend.Application.Services.Interfaces;

namespace IOTBackend.Application
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAPIKeyService, ApiKeyService>();
            services.AddScoped<IConnectionLineService, ConnectionLineService>();
            services.AddScoped<IDeviceInstanceService, DeviceInstanceService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IAwsConnectionService, AwsConnectionService>();

            // singleton service
            services.AddSingleton<IWebSocketService, WebSocketService>();
            services.AddSingleton<ILogicProcessorService, LogicProcessorService>();
        }
    }
}
