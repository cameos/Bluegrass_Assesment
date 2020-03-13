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
    public class CityConfiguration:EntityTypeConfiguration<City>
    {
        public CityConfiguration()
        {
            this.HasKey<Guid>(c => c.CityId).Property(c => c.CityId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();
            this.Property(c => c.CityName).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.Description).HasColumnType("nvarchar").HasMaxLength(550).IsFixedLength().IsOptional();


            this.HasRequired<Province>(c => c.Province).WithMany(c => c.Cities).HasForeignKey<Guid>(c => c.ProvinceId).WillCascadeOnDelete();
            this.HasRequired<Country>(c => c.Country).WithMany(c => c.Cities).HasForeignKey<Guid>(c => c.CountryId).WillCascadeOnDelete();
        }
    }
}
