using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Core.Entities;

namespace JehovaJireh.Core.EntitiesDto
{
	public class DonationDetailsDto
	{
		public DonationDetailsDto()
		{
			this.Images = new List<DonationDetailsImageDto>();
		}
        public virtual Guid Id { get; set; }
        public virtual int Line { get; set; }
		public virtual DonationRefDto Donation { get; set; }
		public virtual RequestedByDto RequestedBy { get; set; }
		public virtual DonationType ItemType { get; set; }
		public virtual string ItemName { get; set; }
		public virtual DonationStatus DonationStatus { get; set; }
		public virtual ICollection<DonationDetailsImageDto> Images { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual UserDto CreatedBy { get; set; }
        public virtual UserDto ModifiedBy { get; set; }
    }
}
