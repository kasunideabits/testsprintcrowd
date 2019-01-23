
namespace SprintCrowd.Backend.Persistence.Configuration
{
    using Microsoft.EntityFrameworkCore;

    public class UserConfig: IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(k => k.Id)
                .ValueGeneratedOnAdd();
        }
    }
}