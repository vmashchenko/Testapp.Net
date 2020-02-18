using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TestApp.DB
{
    public class AppContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private readonly IConfigurationRoot _configuration;

        public AppContextFactory()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public AppDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = _configuration.GetConnectionString(AppDbContext.ConnectionStringName);
            if (connectionString == null)
            {
                throw new ArgumentNullException("DB connection string not found: " + AppDbContext.ConnectionStringName);
            }
                        
            options.UseSqlServer(connectionString, action =>
            {
                action.CommandTimeout(60);
            });

            // use no tracking by default
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new AppDbContext(options.Options);
        }
    }
}
