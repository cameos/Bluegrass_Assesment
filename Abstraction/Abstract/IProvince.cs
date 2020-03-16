using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Abstraction.Abstract
{
    public interface IProvince
    {
        bool insert(Province pr);
        bool update(Province pr);
        bool remove(Province pr);
        bool remove_by_id(Guid id);
        List<Province> all();
        Province show(Province pr);
        Province show_by_id(Guid id);
        Province search_by_name(string name);
    }
}
