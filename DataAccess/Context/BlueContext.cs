using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using DataAccess.Entities;
using DataAccess.EntityConfiguration;
using DataAccess.Migrations;

namespace DataAccess.Context
{
    public class BlueContext:DbContext
    {
        public BlueContext() : base()
        {
            Database.SetInitializer<BlueContext>(new MigrateDatabaseToLatestVersion<BlueContext, Configuration>(useSuppliedContext: true));
        }


        public DbSet<Country> Country { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<Admin> Admin { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new ProvinceConfiguration());
            modelBuilder.Configurations.Add(new CityConfiguration());
            modelBuilder.Configurations.Add(new AddressConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserAddressConfiguration());
            modelBuilder.Configurations.Add(new AdminConfiguration());


            base.OnModelCreating(modelBuilder);
        }
    }
}
