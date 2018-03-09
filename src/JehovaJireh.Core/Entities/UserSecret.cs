using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
    public class UserSecret:EntityBase<int>
    {
        public virtual string Secret { get; set; }
        public virtual string UserName { get; set; }
        public virtual User User { get; set; }
    }
}
