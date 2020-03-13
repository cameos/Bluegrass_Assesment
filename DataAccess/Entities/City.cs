using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class City
    {

        public City()
        {
            this.Addresses = new HashSet<Address>();
        }

        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public string Description { get; set; }


        public Province Province { get; set; }
        public Guid ProvinceId { get; set; }
        public Country Country { get; set; }
        public Guid CountryId { get; set; }

        public ICollection<Address> Addresses { get; set; }
    }
}
