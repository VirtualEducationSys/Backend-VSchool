using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VSchool.Data;
using VSchool.Data.Entities;

namespace VSchool.Data.Mapping
{
    public class LevelMap : EntityConfiguration<Level>
    {
        public override void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.ToTable("Level");
            builder.HasKey(a => a.ID);

            builder.Property(a => a.Label).HasMaxLength(50);
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.Timestamp).IsRowVersion();

            builder.HasMany(a => a.Branchs).WithOne(a => a.Level).HasForeignKey(a => a.LevelID).IsRequired();

            base.Configure(builder);
        }
    }
}
