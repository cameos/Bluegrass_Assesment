using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Abstraction.Abstract
{
    public interface IUser
    {
        bool insert(User user, Address address);
        bool update(User user);
        bool remove(User user);
        bool remove_by_id(Guid id);
        List<User> all();
        User show(User user);
        User show_by_id(Guid id);
        List<User> search_first(PredictiveFilter filter);
        List<User> search_surname(string surname);
        UserCities contact_home();

    }
}
