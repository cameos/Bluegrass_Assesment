using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Address
    {


        public Address()
        {
            this.UserAddresses = new HashSet<UserAddress>();
        }

        public Guid AddressId { get; set; }
        public string AddressNumber { get; set; }
        public string StreetName { get; set; }
        //public string CityName { get; set; }
        public string Surburb { get; set; }
        public string PostalCode { get; set; }


        public Guid CountryId{get;set;}
        public Country Country { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public Province Province { get; set; }
        public Guid ProvinceId { get; set; }

        public ICollection<UserAddress>UserAddresses { get; set; }

    }
}
