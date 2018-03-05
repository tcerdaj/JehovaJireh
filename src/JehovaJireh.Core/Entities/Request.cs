using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
    public class Request :EntityBase<int>
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual DonationType ItemType { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual DonationStatus DonationStatus { get; set; }
    }
}
