using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VSchool.Data;
using VSchool.Data.Entities;

namespace VSchool.Data.Mapping
{
    public class SubjectMap : EntityConfiguration<Subject>
    {
        public override void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Subject");
            builder.HasKey(a => a.ID);

            builder.Property(a => a.Label).HasMaxLength(50);
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.Timestamp).IsRowVersion();

            builder.HasOne(a => a.Branch).WithMany(a => a.Subjects).HasForeignKey(a=>a.BranchID).IsRequired();

            base.Configure(builder);
        }
    }
}
