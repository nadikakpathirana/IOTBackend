using IOTBackend.Domain.DbEntities.Aws;
using Microsoft.EntityFrameworkCore;

namespace IOTBackend.Persistance
{
    public class AwsDbContext : DbContext
    {
        public AwsDbContext() : base() { }

        public AwsDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public AwsDbContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        private static DbContextOptions<AwsDbContext> GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<AwsDbContext>().UseNpgsql(connectionString).Options;
        }
        
        public virtual DbSet<Connection> Connections { get; set; } = null!;
        public virtual DbSet<Device> Devices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            
            modelBuilder.Entity<Connection>(entity =>
            {
                entity.ToTable("connection");

                entity.Property(e => e.Id)
                    .HasColumnType("character varying")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Data)
                    .HasColumnType("jsonb")
                    .HasColumnName("data")
                    .HasDefaultValueSql("'{}'::jsonb");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updatedAt")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("device");

                entity.HasIndex(e => e.ConnectionId, "UQ_50119c4bbc8aaa8b6f4d5e85c2f")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.ConnectionId)
                    .HasColumnType("character varying")
                    .HasColumnName("connectionId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("description");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.LastCheck)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("lastCheck");

                entity.Property(e => e.Location)
                    .HasColumnType("jsonb")
                    .HasColumnName("location")
                    .HasDefaultValueSql("'{}'::jsonb");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.ProjectId).HasColumnName("projectId");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updatedAt")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.VirtualPins)
                    .HasColumnType("jsonb")
                    .HasColumnName("virtualPins")
                    .HasDefaultValueSql("'[]'::jsonb");
            });
        }
    }
}
