using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.CommentAggregate;
using System.Reflection.Emit;

namespace Post.Infrastructure.EntityConfigurations;

public class PostEntityConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.PostAggregate.Post>
{
    public void Configure(EntityTypeBuilder<Domain.AggregatesModel.PostAggregate.Post> builder)
    {
        builder
           .ToTable("Posts");

        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWID()");


        builder.Property(x => x.IsDelete)
                .HasDefaultValue(false);

        builder
            .Property(x => x.Title)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasColumnType("NVARCHAR(MAX)");

        builder
            .Property(x => x.ImageUrl)
            .HasMaxLength(500);


        builder.Property(x => x.CreatedDate)
                .HasDefaultValue(DateTime.Now);

    }
}
