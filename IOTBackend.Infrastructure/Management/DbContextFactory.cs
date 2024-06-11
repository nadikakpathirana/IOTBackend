using IOTBackend.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Infrastructure.Management
{
    public static class DbContextFactory
    {
        public static TContext CreateDbContext<TContext>(string connectionString) where TContext : DbContext, new()
        {
            if (typeof(TContext) == typeof(AppDbContext))
            {
                return (TContext)Activator.CreateInstance(typeof(TContext), new object[] { connectionString });
            } 
            else if(typeof(TContext) == typeof(AwsDbContext))
            {
                return (TContext)Activator.CreateInstance(typeof(TContext), new object[] { connectionString });
            }
            else
            {
                throw new ArgumentException($"Unsupported DbContext type: {typeof(TContext).Name}");
            }
        }

    }
}
