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
        

        public static TContext CreateDbContext<TContext>() where TContext : DbContext, new()
        {
            var builder = new NpgsqlConnectionStringBuilder();

            //Todo: remove them 
            builder.Host = "localhost";
            builder.Port = 5432;
            builder.Database = "IOTBackend";
            builder.Username = "admin";
            builder.Password = "adminadmin";

            if (typeof(TContext) == typeof(AppDbContext))
            {
                return (TContext)Activator.CreateInstance(typeof(TContext), new object[] { builder.ConnectionString });
            }
            else
            {
                throw new ArgumentException($"Unsupported DbContext type: {typeof(TContext).Name}");
            }
        }

    }
}
