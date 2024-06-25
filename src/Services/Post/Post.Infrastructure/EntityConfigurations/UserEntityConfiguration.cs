using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("Users");

        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWID()");

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder
            .Property(x => x.FirstName)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.LastName)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Phone)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Avatar)
            .HasMaxLength(2000);

        builder.Property(x => x.IsDelete)
                .HasDefaultValue(false);

        builder.Property(x => x.CreatedDate)
                .HasDefaultValue(DateTime.Now);
    }
}
