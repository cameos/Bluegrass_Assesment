using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class UserCities
    {
        public List<User> Users{ get; set; }
        public List<Country> Countries { get; set; }
    }
}
