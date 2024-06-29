using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.AggregatesModel.CategoryAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Infrastructure.EntityConfigurations;

public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .ToTable("Categories");

        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.IsDelete)
                .HasDefaultValue(false);

        builder.Property(x => x.CreatedDate)
                .HasDefaultValue(DateTime.Now);


    }
}
