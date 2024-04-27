using IOTBackend.Application.Interfaces;
using IOTBackend.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IOTBackend.API.Configuration
{
    public class RepoConfigurations
    {
        public static void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserService, UserService>();

        }
    }
}
