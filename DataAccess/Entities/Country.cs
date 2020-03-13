using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Country
    {
        public Country()
        {
            this.Provinces = new HashSet<Province>();
            this.Cities = new HashSet<City>();
            this.Addresses = new HashSet<Address>();
        }

        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
        public string Description { get; set; }


        public ICollection<Province> Provinces { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
