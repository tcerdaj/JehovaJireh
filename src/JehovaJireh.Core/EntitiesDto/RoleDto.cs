using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.EntitiesDto
{
	public class RoleDto
	{
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<UserDto> Users { get; set; }
	}
}
