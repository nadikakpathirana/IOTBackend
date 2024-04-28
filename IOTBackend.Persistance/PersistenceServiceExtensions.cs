using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IOTBackend.Persistance
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services,  IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                        configuration.GetConnectionString("AppDbConnection"), 
                        b => b.MigrationsAssembly("IOTBackend.API")
                    );

            });


                    

            return services;
        }
    }
}
