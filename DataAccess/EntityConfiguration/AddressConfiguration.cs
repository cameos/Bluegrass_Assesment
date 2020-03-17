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
    public class AddressConfiguration:EntityTypeConfiguration<Address>
    {
        public AddressConfiguration()
        {
            this.HasKey<Guid>(c => c.AddressId).Property(c => c.AddressId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();
            this.Property(c => c.AddressNumber).HasColumnType("nvarchar").HasMaxLength(30).IsFixedLength().IsRequired();
            this.Property(c=>c.StreetName).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            //this.Property(c=>c.CityName).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c=>c.Surburb).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c=>c.PostalCode).HasColumnType("nvarchar").HasMaxLength(30).IsFixedLength().IsRequired();


            this.HasRequired<City>(c => c.City).WithMany(c => c.Addresses).HasForeignKey<Guid>(c => c.CityId).WillCascadeOnDelete(true);
            this.HasRequired<Province>(c => c.Province).WithMany(c => c.Addresses).HasForeignKey<Guid>(c => c.ProvinceId).WillCascadeOnDelete(false);
            this.HasRequired<Country>(c => c.Country).WithMany(c => c.Addresses).HasForeignKey<Guid>(c => c.CountryId).WillCascadeOnDelete(false);

        }
    }
}
