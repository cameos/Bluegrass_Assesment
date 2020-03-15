using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Abstraction.Abstract
{
    public interface IAdmin
    {
        bool new_admin(Admin admin);
        bool update_admin(Admin admin);
        List<Admin> all_admin();
        Admin show_admin(Guid id);
        Admin show_by_email(string email);
    }
}
