using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
	public class DonationDetails: EntityBase<Guid>
	{
		public DonationDetails()
		{
			this.Images = new List<DonationDetailsImage>();
        }
		public virtual int Line { get; set; }
		public virtual Donation Donation { get; set; }
		public virtual DonationType ItemType { get; set; }
		public virtual User RequestedBy { get; set; }
		public virtual string ItemName { get; set; }
		public virtual DonationStatus DonationStatus { get; set; }
		public virtual ICollection<DonationDetailsImage> Images { get; set; }

        public virtual void AddImage(DonationDetailsImage image)
        {
            if (image != null)
                this.Images.Add(image);
        }
        public virtual void RemoveImage(DonationDetailsImage image)
        {
            if (image != null)
                this.Images.Remove(image);
        }
    }
}
