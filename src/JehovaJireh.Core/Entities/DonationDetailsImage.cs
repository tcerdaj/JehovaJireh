using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
	public class DonationDetailsImage:EntityBase<int>
	{
		public virtual DonationDetails Item { get; set; }
		public virtual string ImageUrl { get; set; }
	}
}
