using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSchool.Data.Entities;

namespace VSchool.Data.Mapping
{
    public class SectionMap : EntityConfiguration <Section>
    {
        public override void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.ToTable("Section");
            builder.HasKey(a => a.ID);
            builder.Property(a => a.Label).HasMaxLength(50);
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.Timestamp).IsRowVersion();

            builder.HasMany(a => a.Sections).WithOne(a => a.ParentSection).HasForeignKey(a => a.ParentSectionID).IsRequired();
            builder.HasOne(a => a.ParentSection).WithMany(a => a.Sections).HasForeignKey(a => a.ParentSectionID);
            builder.HasOne(a => a.Course).WithMany(a => a.Sections).HasForeignKey(a => a.CourseID).IsRequired();

            base.Configure(builder);
        }
    }
}
