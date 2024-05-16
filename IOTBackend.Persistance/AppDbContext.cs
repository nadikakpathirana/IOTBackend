using IOTBackend.Domain.DbEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public AppDbContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        private static DbContextOptions<AppDbContext> GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options;
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<APIKey>? APIKeys { get; set; }
        public DbSet<ConnectionLine>? ConnectionLines { get; set; }
        public DbSet<Device>? Devices { get; set; }
        public DbSet<Project>? Projects { get; set; }

    }
}
