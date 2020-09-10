using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSchool.Data.Entities;

namespace VSchool.Data.Mapping
{
    public class BranchMap : EntityConfiguration<Branch>
    {
        public override void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branch");
            builder.HasKey(a => a.ID);

            builder.Property(a => a.Label).HasMaxLength(50);
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.Timestamp).IsRowVersion();

            builder.HasOne(a => a.Level).WithMany(a => a.Branchs).HasForeignKey(a => a.LevelID).IsRequired();
            builder.HasMany(a => a.Subjects).WithOne(a => a.Branch).HasForeignKey(a => a.BranchID).IsRequired();
            builder.HasMany(a => a.Classes).WithOne(a => a.Branch).HasForeignKey(a => a.BranchID).IsRequired();

            base.Configure(builder);
        }
    }
}
