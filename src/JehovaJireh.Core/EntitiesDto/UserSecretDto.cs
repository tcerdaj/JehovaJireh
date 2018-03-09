using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.EntitiesDto
{
    public class UserSecretDto
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Secret { get; set; }
        public virtual UserDto User { get; set; }
    }
}
