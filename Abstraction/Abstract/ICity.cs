using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Abstraction.Abstract
{
    public interface ICity
    {
        bool insert(City city);
        bool update(City city);
        bool remove(City city);
        bool remove_by_id(Guid id);
        List<City> all();
        City show(City city);
        City show_by_id(Guid id);
        List<City> cities_by_province(Guid id);
    }
}
