using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
	public class Role:EntityBase<int>
	{
		public virtual string Name { get; set; }
	}
}
