using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestApp.Domain.File;

namespace TestApp.DB.Configuration
{
    public class FileConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.ToTable("File");

            //builder.Property(e => e.Content).HasColumnType("image");
        }
    }
}
