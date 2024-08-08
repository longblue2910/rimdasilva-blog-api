using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.AggregatesModel.CourseAggregate;

namespace Post.Infrastructure.EntityConfigurations;

public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Course");

        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
                                //.HasDefaultValueSql("NEWID()");
                                .HasDefaultValueSql("gen_random_uuid()");



        builder.Property(x => x.IsDelete)
                .HasDefaultValue(false);

        builder.Property(x => x.CreatedDate)
                .HasDefaultValue(DateTime.UtcNow.AddHours(7));


    }
}
