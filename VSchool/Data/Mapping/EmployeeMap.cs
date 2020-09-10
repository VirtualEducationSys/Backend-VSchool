using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VSchool.Data.Entities;

namespace VSchool.Data.Mapping
{
    public class EmployeeMap : EntityConfiguration<Employee>
    {
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");
            builder.HasKey(a => a.ID);

            builder.Property(a => a.FirstName).HasMaxLength(20).IsRequired();
            builder.Property(a => a.LastName).HasMaxLength(20).IsRequired();
            builder.Property(a => a.Age);
            builder.Property(a => a.Timestamp).IsRowVersion();



            base.Configure(builder);
        }
    }
}
