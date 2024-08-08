using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.AggregatesModel.CourseAggregate;

namespace Post.Infrastructure.EntityConfigurations;

public class SectionEntityConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.ToTable("Section");

        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
                                    //.HasDefaultValueSql("NEWID()");
                                    .HasDefaultValueSql("gen_random_uuid()");



        builder.Property(x => x.IsDelete)
                .HasDefaultValue(false);

        builder.Property(x => x.CreatedDate)
                .HasDefaultValue(DateTime.UtcNow.AddHours(7));

        builder
            .HasOne(c => c.Course)
            .WithMany(s => s.Sections)
            .HasForeignKey(c => c.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
