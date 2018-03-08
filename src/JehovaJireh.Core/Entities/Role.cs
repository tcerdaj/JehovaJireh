using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
	public class Role:EntityBase<string>
	{
        public Role()
        {
            Users = new List<User>();
        }
		public virtual string Name { get; set; }
        public virtual IList<User> Users { get; set; }
	}
}
