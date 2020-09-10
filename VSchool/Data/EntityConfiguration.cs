using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSchool.Data.Entities;

namespace VSchool.Data
{
    public class EntityConfiguration<T> : IMappingConfiguration, IEntityTypeConfiguration<T> where T : BaseEntity
    {
        protected virtual void PostConfigure(EntityTypeBuilder<T> builder)
        {
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            PostConfigure(builder);
        }
        public virtual void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(this);
        }

    }
}
