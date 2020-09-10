using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VSchool.Data.Entities;

namespace VSchool.Data.Mapping
{
    public class StudentMap : EntityConfiguration<Student>
    {
        public override void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Student");
            builder.HasKey(a => a.ID);

            builder.Property(a => a.FirstName).HasMaxLength(20).IsRequired();
            builder.Property(a => a.LastName).HasMaxLength(20).IsRequired();
            builder.Property(a => a.Age);
            builder.Property(a => a.Timestamp).IsRowVersion();

            builder.HasOne(a => a.Class).WithMany(a => a.Students).HasForeignKey(a => a.ClassID).IsRequired();

            base.Configure(builder);
        }
    }
}
