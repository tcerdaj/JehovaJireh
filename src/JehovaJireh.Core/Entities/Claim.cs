using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
	public class Claim:EntityBase<int>
	{
		public virtual User User { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
	}
}
