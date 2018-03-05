using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
    public class Scheduler:EntityBase<int>
    {
        public virtual Donation Donation { get; set; }
        public virtual DonationDetails Item { get; set; }
        public virtual string Title { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string RecurrenceId { get; set; }
        public virtual string RecurrenceException { get; set; }
        public virtual string RecurrenceRule { get; set; }
        public virtual bool IsAllDay { get; set; }
        public virtual string StartTimezone { get; set; }
        public virtual string EndTimezone { get; set; }
    }
}
