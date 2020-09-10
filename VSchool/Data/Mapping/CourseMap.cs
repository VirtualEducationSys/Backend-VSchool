using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VSchool.Data;
using VSchool.Data.Entities;

namespace VSchool.Data.Mapping
{
    public class CourseMap : EntityConfiguration<Course>
    {
        public override void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Course");
            builder.HasKey(a => a.ID);

            builder.Property(a => a.Label).HasMaxLength(50);
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.Duration).IsRequired();
            builder.Property(a => a.Timestamp).IsRowVersion();

            builder.HasOne(a => a.Subject);
            builder.HasMany(a => a.Sections).WithOne(a => a.Course).HasForeignKey(a => a.CourseID);
            base.Configure(builder);
        }
    }
}
