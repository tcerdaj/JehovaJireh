using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
	public class Item: EntityBase<Guid>
	{
		public virtual int Line { get; set; }
		public virtual DonationType ItemType { get; set; }
		public virtual User RequestedBy { get; set; }
		public virtual string ItemName { get; set; }
		public virtual DonationStatus DonationStatus { get; set; }
    }
}
