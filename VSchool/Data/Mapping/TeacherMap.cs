using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSchool.Data.Entities;

namespace VSchool.Data.Mapping
{
    public class TeacherMap : EntityConfiguration<Teacher>
    {
        public override void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teacher");
            builder.HasKey(a => a.ID);
            builder.Property(a => a.Timestamp).IsRowVersion();

            builder.HasMany(a => a.Classes).WithOne(a => a.Teacher).HasForeignKey(a => a.TeacherID).IsRequired();
            builder.HasOne(a => a.Employee).WithOne(a => a.Teacher).HasForeignKey<Teacher>(a => a.EmployeeID).IsRequired();

            base.Configure(builder);
        }
    }
}
