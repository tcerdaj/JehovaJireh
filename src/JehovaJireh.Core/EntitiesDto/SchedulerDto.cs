using JehovaJireh.Core.EntitiesDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.EntitiesDto
{
    public class SchedulerDto
    {
        public virtual int Id { get; set; }
        public virtual DonationDto Donation { get; set; }
        public virtual DonationDetailsDto Item { get; set; }
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
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual UserRefDto CreatedBy { get; set; }
        public virtual UserRefDto ModifiedBy { get; set; }

        public virtual List<JsonConverterImageDto> Images {
            get
            {
                var results = new List<JsonConverterImageDto>();
                try
                {
                    results = string.IsNullOrEmpty(ImageUrl) ? new List<JsonConverterImageDto>() : JsonConvert.DeserializeObject<List<JsonConverterImageDto>>(ImageUrl);
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return results;
            }
        }
    }
}
