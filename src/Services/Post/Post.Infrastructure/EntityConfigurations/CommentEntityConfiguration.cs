using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.AggregatesModel.CommentAggregate;

namespace Post.Infrastructure.EntityConfigurations;

public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .ToTable("Comments");

        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
                                    .HasDefaultValueSql("gen_random_uuid()");

        //.HasDefaultValueSql("NEWID()");


        builder.Property(x => x.IsDelete)
                .HasDefaultValue(false);

        builder.Property(x => x.CreatedDate)
                .HasDefaultValue(DateTime.UtcNow.AddHours(7));

        builder
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
