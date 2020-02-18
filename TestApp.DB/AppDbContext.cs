using Microsoft.EntityFrameworkCore;
using TestApp.DB.Configuration;

namespace TestApp.DB
{
    public class AppDbContext : DbContext
    {        
        public const string ConnectionStringName = "defaultConnection";

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Mappings(modelBuilder);
        }

        private void Mappings(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileConfiguration());
            modelBuilder.ApplyConfiguration(new FieldConfiguration());
        }
    }
}
