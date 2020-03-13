using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User
    {

        public User()
        {
            this.UserAddresses = new HashSet<UserAddress>();
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte[] Avatar { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }

        public ICollection<UserAddress> UserAddresses { get; set; }
    }
}
