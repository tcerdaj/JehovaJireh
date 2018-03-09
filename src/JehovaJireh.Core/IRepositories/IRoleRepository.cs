using JehovaJireh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.IRepositories
{
    public interface IRoleRepository:IRepository<Role, string>
    {
        List<Role> GetRolesByUserId(string userId);
    }
}
