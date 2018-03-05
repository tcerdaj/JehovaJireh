using JehovaJireh.Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.EntitiesDto
{
    public class DonationRequestedDto
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual int DonationId { get; set; }
        public virtual Guid ItemId { get; set; }
        public virtual int RequestedBy { get; set; }
        public virtual DonationStatus DonationStatus { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual bool IsAnItem { get { return ItemId != Guid.Empty; } }
        public virtual IEnumerable<JsonConverterImageDto> Images { get; set; }
        public virtual int CreatedBy { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual string UserImageUrl { get; set; }
    }
}
