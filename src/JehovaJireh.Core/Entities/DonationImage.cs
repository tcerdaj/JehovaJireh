using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
	public class DonationImage:EntityBase<int>
	{
		public virtual Donation Donation { get; set; }
		public virtual string ImageUrl { get; set; }
	}
}
