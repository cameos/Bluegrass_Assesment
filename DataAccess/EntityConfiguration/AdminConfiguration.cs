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
    public class AdminConfiguration:EntityTypeConfiguration<Admin>
    {
        public AdminConfiguration()
        {
            this.HasKey<Guid>(c => c.AdminId).Property(c => c.AdminId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();
            this.Property(c => c.FirstName).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.LastName).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.Salt).HasColumnType("text").IsRequired();
            this.Property(c => c.Password).HasColumnType("text").IsRequired();
            this.Property(c => c.Email).HasColumnType("nvarchar").HasMaxLength(300).IsFixedLength().IsRequired();

        }
    }
}
