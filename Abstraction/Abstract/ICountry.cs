using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Abstraction.Abstract
{
    public interface ICountry
    {
        bool insert(Country cou);
        bool update(Country cou);
        bool remove(Country cou);
        bool remove_by_id(Guid id);
        List<Country> all();
        Country show(Country cou);
        Country show_by_id(Guid id);
        Country search_by_name(string name);
    }
}
