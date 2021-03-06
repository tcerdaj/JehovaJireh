﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace JehovaJireh.Core.Entities
{
	public class User : EntityBase<int>, IUser
	{
		#region constructor
		public User()
		{
			Roles = new List<Role>();
            Claims = new List<Claim>();
            Logins = new List<Login>();
		}
		#endregion
		#region Properties
		public virtual string ImageUrl { get; set; }
		public virtual string UserName { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string Gender { get; set; }
		public virtual string PasswordHash { get; set; }
		public virtual string SecurityStamp { get; set; }
		public virtual string Email { get; set; }
		public virtual string Address { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string Zip { get; set; }
		public virtual string PhoneNumber { get; set; }
		public virtual bool Active { get; set; }
		public virtual string ConfirmationToken { get; set; }
		public virtual bool IsConfirmed { get; set; }
		public virtual bool IsChurchMember { get; set; }
		public virtual bool LockoutEnabled { get; set; }
		public virtual bool TwoFactorEnabled { get; set; }
		public virtual int FailedCount { get; set; }
		public virtual string ChurchName { get; set; }
		public virtual string ChurchAddress { get; set; }
		public virtual string ChurchPhone { get; set; }
		public virtual string ChurchPastor { get; set; }
		public virtual bool NeedToBeVisited { get; set; }
		public virtual string Comments { get; set; }
		public virtual DateTimeOffset LockoutEndDate { get; set; }
        public virtual DateTime? LastLogin { get; set; }
		public virtual IList<Role> Roles { get; set; }
        public virtual IList<Claim> Claims { get; set; }
        public virtual IList<Login> Logins { get; set; }
        public virtual UserSecret Secret { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual string Country { get; set; }
        string IUser<string>.Id
		{
			get { return this.Id.ToString(); }
		}
        #endregion
        #region Methods

        public virtual void RemoveLogin(Login login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            Logins.Remove(login);
        }

        public virtual void AddLogin(Login login)
        {
            if (login == null)
                throw new ArgumentNullException("login");
            login.User = this;
            Logins.Add(login);
        }
        public virtual void RemoveClaim(Claim claim)
        {
            if (claim == null)
                throw new ArgumentNullException("claim");

            Claims.Remove(claim);
        }

        public virtual void AddClaim(Claim claim)
        {
            if (claim == null)
                throw new ArgumentNullException("claim");

            claim.User = this;
            Claims.Add(claim);
        }

        public virtual void AddClaim(List<Claim> claims)
        {
            if (claims == null)
                throw new ArgumentNullException("claims");

            foreach (var claim in claims)
            {
                claim.User = this;
                Claims.Add(claim);
            }
        }

        public virtual void RemoveRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            Roles.Remove(role);
        }

        public virtual void AddRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            Roles.Add(role);
        }
		public virtual async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}

		public virtual void SetStatus(Boolean active)
		{
			this.Active = active;
		}

		public virtual User ToObject(string json)
		{
			try
			{
				var result = JsonConvert.DeserializeObject<User>(json);
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
			return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
		}

        public virtual async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string defaultAuthenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, defaultAuthenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        #endregion
    }
}
