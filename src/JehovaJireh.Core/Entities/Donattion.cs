using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JehovaJireh.Core.Entities
{
	public class Donation: EntityBase<int>
	{
		public Donation()
		{
			this.DonationDetails = (ICollection<DonationDetails>)new List<DonationDetails>();
            this.Images = new List<DonationImage>();
        }

		public virtual User RequestedBy { get; set; }
        public virtual Request Request { get; set; }
        public virtual string Title { get; set; }
		public virtual string Description { get; set; }
		public virtual decimal Amount { get; set; }
		public virtual DateTime? ExpireOn { get; set; }
		public virtual DateTime DonatedOn { get; set; }
		public virtual DonationStatus DonationStatus { get; set; }
		public virtual ICollection<DonationDetails> DonationDetails { get; set; }
        public virtual ICollection<DonationImage> Images { get; set; }

        #region Image
        public virtual void AddImage(DonationImage image)
        {
            if (image != null)
                this.Images.Add(image);
        }
        public virtual void RemoveImage(DonationImage image)
        {
            if (image != null)
                this.Images.Remove(image);
        }

        #endregion

        #region Items
        public virtual void AddIItem(DonationDetails item)
        {
            if (item != null)
                this.DonationDetails.Add(item);
        }
        public virtual void RemoveItem(DonationDetails item)
        {
            if (item != null)
                this.DonationDetails.Remove(item);
        }
        #endregion

        public virtual Donation ToObject(string json)
		{
			try
			{
				var result = JsonConvert.DeserializeObject<Donation>(json);
				return result;
			}
			catch (System.Exception ex)
			{
				Console.WriteLine("Error in BaseEntity line 25, method ToObject(): " + ex.Message);
				throw ex;
			}
		}

		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
