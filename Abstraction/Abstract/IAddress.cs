using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Abstraction.Abstract
{
    public interface IAddress
    {
        //bool insert(Address ad);
        bool update(Address ad);
        //bool remove(Address ad);
        //bool remove_by_id(Guid id);
        //List<Address> all();
        //Address show(Address ad);
        //Address show_by_id(Guid id);
    }
}
