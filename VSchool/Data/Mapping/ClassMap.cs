using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSchool.Data.Entities;

namespace VSchool.Data.Mapping
{
    public class ClassMap : EntityConfiguration<Class>
    {
        public override void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable("Class");
            builder.HasKey(a => a.ID);

            builder.Property(a => a.Label).HasMaxLength(50);
            builder.Property(a => a.Timestamp).IsRowVersion();

            builder.HasOne(a => a.Branch).WithMany(a => a.Classes).HasForeignKey(a => a.BranchID).IsRequired();
            builder.HasOne(a => a.Teacher).WithMany(a => a.Classes).HasForeignKey(a => a.TeacherID).IsRequired();
            builder.HasMany(a => a.Students).WithOne(a => a.Class).HasForeignKey(a => a.ClassID).IsRequired();

            base.Configure(builder);
        }
    }
}
