using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Abstraction.Abstract
{
    public interface IUserAddress
    {
        bool insert(UserAddress ua);
        bool update(UserAddress ua);
        bool remove(UserAddress ua);
        bool remove_by_id(Guid id);
        List<UserAddress> all();
        UserAddress show(UserAddress ua);
        UserAddress show_by_id(Guid id);
    }
}
