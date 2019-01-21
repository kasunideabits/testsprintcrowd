namespace SprintCrowdBackEnd.Persistence.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using SprintCrowdBackEnd.Models;

    public class ProfilePictureConfig : IEntityTypeConfiguration<ProfilePicture>
    {

        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProfilePicture> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(k => k.UserId).IsRequired();
            builder.HasOne(u => u.User).WithOne(u => u.ProfilePicture).HasForeignKey<ProfilePicture>(u => u.UserId);
        }
       

    }
}