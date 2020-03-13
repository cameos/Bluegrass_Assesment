using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Province
    {

        //constructor
        public Province()
        {
            this.Cities = new HashSet<City>();
            this.Addresses = new HashSet<Address>();
        }


        //fields
        public Guid ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string Description { get; set; }


        //navigation properties
        public Guid CountryId { get; set; }
        public Country Country { get; set; }


        public ICollection<City> Cities { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
