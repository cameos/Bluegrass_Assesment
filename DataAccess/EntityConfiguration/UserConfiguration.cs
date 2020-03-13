using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using System.Data;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.EntityConfiguration
{
    public class UserConfiguration:EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.HasKey<Guid>(c => c.UserId).Property(c => c.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();
            this.Property(c => c.FirstName).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c=>c.LastName).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c=>c.ID).HasColumnType("nvarchar").HasMaxLength(20).IsFixedLength().IsRequired();
            this.Property(c=>c.Phone).HasColumnType("nvarchar").HasMaxLength(30).IsFixedLength().IsRequired();
            this.Property(c=>c.Email).HasColumnType("nvarchar").HasMaxLength(300).IsFixedLength().IsRequired();
            this.Property(c=>c.Gender).HasColumnType("nvarchar").HasMaxLength(10).IsFixedLength().IsRequired();
            this.Property(c => c.Status).HasColumnType("nvarchar").HasMaxLength(10).IsFixedLength().IsRequired();
            this.Property(c => c.Avatar).HasColumnType("image").IsOptional();


        }
    }
}
