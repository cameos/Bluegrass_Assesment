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
    public class UserAddressConfiguration:EntityTypeConfiguration<UserAddress>
    {
        public UserAddressConfiguration()
        {
            this.HasKey<Guid>(c => c.UserAddressId).Property(c => c.UserAddressId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();


            this.HasRequired<User>(c => c.User).WithMany(c => c.UserAddresses).HasForeignKey<Guid>(c => c.UserId).WillCascadeOnDelete(true);
            this.HasRequired<Address>(c => c.Address).WithMany(c => c.UserAddresses).HasForeignKey<Guid>(c => c.AddressId).WillCascadeOnDelete(true);
        }
    }
}
