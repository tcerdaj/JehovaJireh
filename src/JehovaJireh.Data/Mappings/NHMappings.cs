﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using JehovaJireh.Core.Entities;
using JehovaJireh.Core.EntitiesDto;

namespace JehovaJireh.Data.Mappings
{
	public class UserMap : ClassMap<User>
	{
		public UserMap()
		{
			Table("Users");
            Id(x => x.Id)
				.Column("UserId")
				.GeneratedBy.Increment();
			Map(x => x.ImageUrl);
			Map(x => x.UserName);
			Map(x => x.FirstName);
			Map(x => x.LastName);
			Map(x => x.Gender);
			Map(x => x.PasswordHash);
			Map(x => x.SecurityStamp);
			Map(x => x.Email);
			Map(x => x.Address);
			Map(x => x.City);
			Map(x => x.State);
			Map(x => x.Zip);
			Map(x => x.PhoneNumber).Column("Phone");
			Map(x => x.Active);
			Map(x => x.LockoutEnabled);
			Map(x => x.TwoFactorEnabled);
			Map(x => x.FailedCount);
			Map(x => x.ConfirmationToken);
			Map(x => x.IsConfirmed);
			Map(x => x.IsChurchMember);
			Map(x => x.ChurchName);
			Map(x => x.ChurchAddress);
			Map(x => x.ChurchPhone);
			Map(x => x.ChurchPastor);
			Map(x => x.NeedToBeVisited);
			Map(x => x.Comments);
            Map(x => x.BirthDate);
            Map(x => x.Country);
            HasManyToMany(x => x.Roles)
                .Cascade.All()
                .ParentKeyColumn("UserId")
                .ChildKeyColumn("RoleId")
                .Table("UserRoles");
            HasMany(x => x.Logins)
                .Cascade
                .AllDeleteOrphan()
                .Inverse()
                .KeyColumn("UserId");
            HasMany(x => x.Claims)
               .Cascade
                .AllDeleteOrphan()
                .Inverse()
                .KeyColumn("UserId");
            HasOne(x => x.Secret)
                .Cascade
                .All();
            References(x => x.CreatedBy)
				.Column("CreatedBy")
				.ForeignKey("CreatedBy")
                .Cascade.Delete();
			References(x => x.ModifiedBy)
				.Column("ModifiedBy")
				.ForeignKey("ModifiedBy")
                .Cascade.Delete();
            Map(x => x.CreatedOn).Default("getdate()");
            Map(x => x.ModifiedOn).Default("getdate()");
            Map(x => x.LastLogin)
                .Column("LastLoginDatetime").Default("getdate()");
        }
	}

    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Table("Roles");
            Id(x => x.Id);
            Map(x => x.Name).Not.Nullable().UniqueKey("UQ_Roles_RoleName");
            HasManyToMany(x => x.Users)
                .ParentKeyColumn("RoleId")
                .ChildKeyColumn("UserId")
                .Cascade.SaveUpdate()
                .Inverse()
                .Table("UserRoles");
        }
    }

    public class ClaimMap : ClassMap<Claim>
    {
        public ClaimMap()
        {
            Table("UserClaims");
            Id(x => x.Id).Column("ClaimId")
                .GeneratedBy
                .Increment();
            Map(x => x.ClaimType);
            Map(x => x.ClaimValue);
            References(x => x.User)
                .Column("UserId");
        }
    }

    public class LoginMap : ClassMap<Login>
    {
        public LoginMap()
        {
            Table("UserLogins");
            Id(x => x.Id)
              .Column("LoginId")
              .GeneratedBy
              .Increment(); 
            Map(x => x.LoginProvider);
            Map(x => x.ProviderKey);
            References(x => x.User).Column("UserId");
        }
    }

    public class UserSecretMap : ClassMap<UserSecret>
    {
        public UserSecretMap()
        {
            Table("UserSecrets");
            Id(x => x.Id).Column("UserId");
            Map(x => x.Secret);
            References(x => x.User)
               .Column("UserId")
               .ForeignKey();
        }
    }

    public class DonationMap : ClassMap<Donation>
	{
		public DonationMap()
		{
			Table("Donations");
			Id(x => x.Id)
				.Column("DonationId")
				.GeneratedBy.Increment();
			Map(x => x.Title).Not.Nullable();
            Map(x => x.Description).Not.Nullable();
			Map(x => x.Amount);
			Map(x => x.ExpireOn);
			Map(x => x.DonationStatus).CustomType<DonationStatus>().Not.Nullable();
            Map(x => x.CreatedOn).Default("getdate()").Not.Nullable();
			Map(x => x.ModifiedOn).Nullable();
			Map(x => x.DonatedOn).Default("getdate()").Not.Nullable();
            HasMany(x => x.DonationDetails)
                .KeyColumn("DonationId")
                .Inverse()
                .Cascade.All()
                .AsBag()
                .Not.LazyLoad();
            HasMany(x => x.Images)
               .KeyColumn("DonationId")
               .Inverse()
               .Cascade.All();
            References(x => x.RequestedBy)
                .Column("RequestedBy")
                .ForeignKey("RequestedBy");
            References(x => x.Request)
               .Column("RequestId")
               .ForeignKey("RequestId")
               .Nullable();
            References(x => x.CreatedBy)
                .Column("CreatedBy")
                .ForeignKey("CreatedBy");
            References(x => x.ModifiedBy)
                .Column("ModifiedBy")
                .ForeignKey("ModifiedBy");
        }
	}

    public class DonationDetailMap : ClassMap<DonationDetails>
    {
        public DonationDetailMap()
        {
            Table("DonationDetails");
            Id(x => x.Id)
                .Column("ItemId")
                .GeneratedBy.Guid();
            References(x => x.Donation)
                .Column("DonationId");
            HasMany(x => x.Images)
                .KeyColumn("ItemId")
                .Inverse()
                .Cascade.All();
            Map(x => x.Line).Column("Line");
            Map(x => x.ItemType)
                .CustomType<DonationType>();
            Map(x => x.ItemName);
            Map(x => x.DonationStatus)
                .CustomType<DonationStatus>();
            Map(x => x.CreatedOn);
            Map(x => x.ModifiedOn);
            References(x => x.RequestedBy)
                .Column("RequestedBy")
                .ForeignKey();
            References(x => x.CreatedBy)
                .Column("CreatedBy")
                .ForeignKey();
            References(x => x.ModifiedBy)
                .Column("ModifiedBy")
                .ForeignKey();
        }
    }

    public class DonationRequestedMap : ClassMap<DonationRequested>
    {
        public DonationRequestedMap()
        {
            Table("vw_DonationRequested");
            ReadOnly();
            Id(x => x.Id).Column("Id");
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.ImageUrl);
            Map(x => x.DonationStatus)
                .CustomType<DonationStatus>();
            Map(x => x.DonationId);
            Map(x => x.ItemId);
            Map(x => x.RequestedBy);
            Map(x => x.CreatedBy);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Email);
            Map(x => x.Phone);
            Map(x => x.UserImageUrl);
        }
    }
    public class RequestMap : ClassMap<Request>
    {
        public RequestMap()
        {
            Table("Request");
            Id(x => x.Id)
                .Column("RequestId")
                .GeneratedBy.Increment();
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.DonationStatus)
                .CustomType<DonationStatus>()
                .Column("RequestStatus");
            Map(x => x.ItemType)
                .CustomType<DonationType>();
            Map(x => x.ImageUrl);
            Map(x => x.CreatedOn);
            Map(x => x.ModifiedOn);
            References(x => x.CreatedBy)
                .Column("CreatedBy")
                .ForeignKey("CreatedBy");
            References(x => x.ModifiedBy)
                .Column("ModifiedBy")
                .ForeignKey("ModifiedBy");
        }
    }

   

	public class DonationImageMap : ClassMap<DonationImage>
	{
		public DonationImageMap()
		{
			Table("DonationImages");
			Id(x => x.Id)
				.Column("DonationImageId")
				.GeneratedBy.Increment();
			Map(x => x.ImageUrl);
			Map(x => x.CreatedOn);
			Map(x => x.ModifiedOn);
            References(x => x.Donation)
                .Column("DonationId")
                .ForeignKey("DonationId");
            References(x => x.CreatedBy)
				.Column("CreatedBy")
				.ForeignKey();
			References(x => x.ModifiedBy)
				.Column("ModifiedBy")
				.ForeignKey();
		}
	}

    public class DonationDetailsImageMap : ClassMap<DonationDetailsImage>
    {
        public DonationDetailsImageMap()
        {
            Table("DonationDetailImages");
            Id(x => x.Id)
                .Column("DonationImageId")
                .GeneratedBy.Increment();
            Map(x => x.ImageUrl);
            Map(x => x.CreatedOn);
            Map(x => x.ModifiedOn);
            References(x => x.Item)
                .Column("ItemId")
                .ForeignKey("ItemId");
            References(x => x.CreatedBy)
                .Column("CreatedBy")
                .ForeignKey();
            References(x => x.ModifiedBy)
                .Column("ModifiedBy")
                .ForeignKey();
        }
    }

    public class SchedulerMap : ClassMap<Scheduler>
    {
        public SchedulerMap()
        {
            Table("Scheduler");
            Id(x => x.Id)
                .Column("SchedulerId")
                .GeneratedBy.Increment();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.ImageUrl);
            Map(x => x.Description);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.RecurrenceId);
            Map(x => x.RecurrenceException);
            Map(x => x.RecurrenceRule);
            Map(x => x.IsAllDay);
            Map(x => x.CreatedOn).Default("getdate()").Not.Nullable();
            Map(x => x.ModifiedOn).Default("getdate()").Nullable();
            Map(x => x.StartTimezone).Nullable();
            Map(x => x.EndTimezone).Nullable();
            References(x => x.Donation)
                .Column("DonationId")
                .Not.Nullable()
                .ForeignKey("DonationId");
            References(x => x.Item)
                .Column("ItemId")
                .Nullable()
                .ForeignKey();
            References(x => x.CreatedBy)
                .Column("CreatedBy")
                .Not.Nullable()
                .ForeignKey();
            References(x => x.ModifiedBy)
                .Column("ModifiedBy")
                .Nullable()
                .ForeignKey();
        }
    }
}
