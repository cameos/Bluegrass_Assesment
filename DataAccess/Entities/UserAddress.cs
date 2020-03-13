using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class UserAddress
    {
        public Guid UserAddressId { get; set; }



        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
    }
}
