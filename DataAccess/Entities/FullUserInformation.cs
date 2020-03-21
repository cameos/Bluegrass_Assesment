using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class FullUserInformation
    {
        public User User { get; set; }
        public Address Address { get; set; }
        public Country Country { get; set; }
        public Province Province { get; set; }
        public City City { get; set; }
    }
}
