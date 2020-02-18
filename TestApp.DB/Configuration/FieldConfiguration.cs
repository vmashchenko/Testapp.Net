using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestApp.Domain.File;

namespace TestApp.DB.Configuration
{
    public class FieldConfiguration : IEntityTypeConfiguration<FieldEntity>
    {
        public void Configure(EntityTypeBuilder<FieldEntity> builder)
        {
            builder.ToTable("FieldEntity");

            builder.HasOne(d => d.File)
                .WithMany()
                .HasForeignKey(d => d.FileId);
        }
    }
}
