﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Core.Entities;

namespace JehovaJireh.Core.EntitiesDto
{
	public class UserDto
	{
		#region Properties
        public virtual int Id { get; set; }
		public virtual string ImageUrl { get; set; }
		public virtual string UserName { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string Gender { get; set; }
		public virtual string Email { get; set; }
		public virtual string Address { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string Zip { get; set; }
        public virtual string Country { get; set; }
        public virtual string PhoneNumber { get; set; }
		public virtual bool Active { get; set; }
		public virtual bool LockoutEnabled { get; set; }
		public virtual bool TwoFactorEnabled { get; set; }
		public virtual int FailedCount { get; set; }
		public virtual bool IsConfirmed { get; set; }
		public virtual bool IsChurchMember { get; set; }
		public virtual string ChurchName { get; set; }
		public virtual string ChurchAddress { get; set; }
		public virtual string ChurchPhone { get; set; }
		public virtual string ChurchPastor { get; set; }
		public virtual bool NeedToBeVisited { get; set; }
		public virtual string Comments { get; set; }
		public virtual DateTimeOffset LockoutEndDate { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual UserRefDto CreatedBy { get; set; }
        public virtual UserRefDto ModifiedBy { get; set; }
        public virtual DateTime LastLogin { get; set; }
        public virtual DateTime BirthDate { get; set; }

        #endregion
    }
}
