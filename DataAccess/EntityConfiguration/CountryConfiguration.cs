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
    public class CountryConfiguration:EntityTypeConfiguration<Country>
    {
        public CountryConfiguration()
        {
            this.HasKey<Guid>(c => c.CountryId).Property(c => c.CountryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();
            this.Property(c => c.CountryName).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.Description).HasColumnType("nvarchar").HasMaxLength(550).IsFixedLength().IsOptional();

        }
    }
}
