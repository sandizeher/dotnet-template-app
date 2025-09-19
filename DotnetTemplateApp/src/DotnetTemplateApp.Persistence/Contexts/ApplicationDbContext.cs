using DotnetTemplateApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DotnetTemplateApp.Persistence.Contexts
{
    public partial class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine("Configuring...");
            if (!optionsBuilder.IsConfigured)
            {
                Console.WriteLine("Configuring local DB context...");
                ConfigureLocalDbContext(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Account

            SetUser(modelBuilder);
            SetUserAccount(modelBuilder);
            
            #endregion
        }

        private static void SetBaseModel<T>(EntityTypeBuilder<T> entity, string primaryKeyName) where T : Model
        {
            entity.HasKey(e => e.Id)
                .HasName(primaryKeyName);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<GuidValueGenerator>()
                .HasColumnName("id");

            entity.Property(e => e.DateCreated)
                .HasColumnName("date_created");

            entity.Property(e => e.DateModified)
                .HasColumnName("date_modified");
        }

        private static void ConfigureLocalDbContext(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.local.json")
                .Build();

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetSection("DbSettings:ConnectionString").Value!);
            
            using var dataSource = dataSourceBuilder.Build();
            optionsBuilder.UseNpgsql(
                dataSource,
                options => options.EnableRetryOnFailure());
        }
    }
}
