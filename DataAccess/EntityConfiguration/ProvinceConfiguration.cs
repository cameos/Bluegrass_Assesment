using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities;

namespace DataAccess.EntityConfiguration
{
    public class ProvinceConfiguration:EntityTypeConfiguration<Province>
    {
        public ProvinceConfiguration()
        {
            this.HasKey<Guid>(c => c.ProvinceId).Property(c => c.ProvinceId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();
            this.Property(c => c.ProvinceName).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.Description).HasColumnType("nvarchar").HasMaxLength(550).IsFixedLength().IsOptional();

            this.HasRequired<Country>(c => c.Country).WithMany(c => c.Provinces).HasForeignKey<Guid>(c => c.ProvinceId).WillCascadeOnDelete();
        }
    }
}
