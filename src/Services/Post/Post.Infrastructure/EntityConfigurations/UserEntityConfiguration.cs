using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {

        builder
            .Property(x => x.FullName)
            .HasMaxLength(200)
            .IsRequired();

    }
}
