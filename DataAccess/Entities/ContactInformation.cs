using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class ContactInformation
    {
        public User User { get; set; }
        public Address Address { get; set; }
    }
}
