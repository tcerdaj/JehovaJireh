﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JehovaJireh.Core.Entities;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NHibernate;
using NHibernate.Linq;

namespace JehovaJireh.Data.Repositories
{
	public class UserRepository:NHRepository<User, int>, IUserRepository, IUserStore<User>, 
         IUserPasswordStore<User>, IUserSecurityStampStore<User>, IQueryableUserStore<User>, 
         IUserEmailStore<User>,IUserLockoutStore<User, string>, Microsoft.AspNet.Identity.IUserRoleStore<User, string>, 
         IUserTwoFactorStore<User,string>,IUserPhoneNumberStore<User,string>, IUserLoginStore<User,string>, IDisposable
    {
		ISession session;
		ILogger log;

		public UserRepository(ISession session, ExceptionManager exManager, ILogger log)
			: base(session, exManager, log)
		{
			this.log = log;
		}

		public UserRepository(ISession session)
			: base(session)
		{
			this.session = session;
		}

		public UserRepository()
		{

		}

		
		public User GetByUserName(string userName)
		{
            User user;
            try
            {
                user  = (from u in Query() where u.UserName == userName select u).SingleOrDefault();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return user;
        }

		public User GetByEmail(string email)
		{
			return (from u in Query() where u.Email == email select u).SingleOrDefault();
		}

        public async Task<bool> AddToRoleAsync(string userId, string role)
        {
            var success = true;

            try
            {
                var user = await this.GetById(userId);
                if (user != null && user.Roles == null)
                {
                    user.Roles = new List<Role>();
                    var _role = session.Query<Role>().Where(x => x.Id == role).FirstOrDefault();
                    if (_role != null)
                        user.Roles.Add(_role);
                }
                else if (!user.Roles.Any(x => x.Id == role))
                {
                    var _role = session.Query<Role>().Where(x => x.Id == role).FirstOrDefault();
                    if (_role != null)
                        user.Roles.Add(_role);
                }
                if (user != null)
                    await this.UpdateAsync(user);
            }
            catch (System.Exception)
            {
                success = false;
            }

            return success;
        }

		public User GetByConfirmationToken(string token)
		{
			return (from u in Query() where u.ConfirmationToken == token select u).SingleOrDefault();
		}

		public IQueryable<User> Users
		{
			get { return Query().AsQueryable<User>(); }
		}

		public Task CreateAsync(User user)
		{
			if ((object)user == null)
				throw new ArgumentNullException("user");

			var result = string.Empty;

			try
			{
				this.Update(user);

				result = user.Id.ToString();
			}
			catch (System.Exception ex)
			{
				log.Error(ex);
				throw ex;
			}

			return Task.FromResult(result);
		}

		public Task DeleteAsync(User user)
		{
			var result = string.Empty;
			this.Delete(user);
			return Task.FromResult(result);
		}

		public Task<User> FindByNameAsync(string userName)
		{
			return Task.FromResult(this.GetByUserName(userName));
		}

		public Task UpdateAsync(User user)
		{
			if ((object)user == null)
				throw new ArgumentNullException("user");

            Session.Clear();
            Session.Flush();

            this.Update(user);
			return Task.FromResult(0);
		}

		public Task<string> GetPasswordHashAsync(User user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.PasswordHash);
		}

		public Task<bool> HasPasswordAsync(User user)
		{
			return Task.FromResult(user.PasswordHash != null);
		}

		public Task SetPasswordHashAsync(User user, string passwordHash)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			user.PasswordHash = passwordHash;

			return Task.FromResult(0);
		}

		public Task<string> GetSecurityStampAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.SecurityStamp);
		}

		public Task SetSecurityStampAsync(User user, string stamp)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			user.SecurityStamp = stamp;

			return Task.FromResult(0);
		}

		public Task<User> FindByEmailAsync(string email)
		{
			if (string.IsNullOrEmpty(email))
				throw new ArgumentNullException("email");

			var result = (from u in this.Query() where u.Email == email select u).FirstOrDefault();

			return Task<User>.FromResult(result);
		}

		public Task<string> GetEmailAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			var result = (from u in this.Query() where u.Email == user.Email select u).FirstOrDefault() != null;

			return Task<bool>.FromResult(result);

		}

		public Task SetEmailAsync(User user, string email)
		{
			throw new NotImplementedException();
		}

		public Task SetEmailConfirmedAsync(User user, bool confirmed)
		{
			throw new NotImplementedException();
		}
		

		public Task<User> FindByIdAsync(string userId)
		{
			if (userId == string.Empty)
				throw new ArgumentNullException("user");

			return Task.FromResult(this.GetById(userId)).Result;
		}

		public Task<User> FindByIdAsync(int userId)
		{
			throw new NotImplementedException();
		}

		public Task<User> GetById(string id)
		{
			if (id == string.Empty)
				throw new ArgumentNullException("Id");

            Guid guidOutput = Guid.Empty;
            bool isValid = Guid.TryParse(id, out guidOutput);
            id = isValid ? "0" : id;

            return Task.FromResult((from u in this.Query() where u.Id == int.Parse(id) select u).SingleOrDefault());
		}

		public Task AddToRoleAsync(User user, string roleId)
		{
			if (user == null)
				throw new NullReferenceException("user");

			if (string.IsNullOrEmpty(roleId))
				throw new NullReferenceException("roleIdoleName");

            try
            {
                var isInRole =  IsInRoleAsync(user, roleId);
                if (!isInRole.Result)
                {
                    var role = Session.Query<Role>().Where(x => x.Id == roleId || x.Name == roleId).FirstOrDefault();
                    if (role != null)
                    {
                        user.AddRole(role);
                        Update(user);
                    }
                }
            }
            catch (System.Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
			return Task.FromResult(roleId);
		}


        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user == null)
                throw new NullReferenceException("user");

            if (string.IsNullOrEmpty(roleName))
                throw new NullReferenceException("roleName");

            var exist = IsInRoleAsync(user, roleName);

            if (exist.Result)
            {
                var index = user.Roles.ToList().FindIndex(x => x.Id == roleName || x.Name == roleName);
                user.Roles.RemoveAt(index);
                this.UpdateAsync(user);
            }

			return Task.FromResult(user);
		}

		public Task<IList<string>> GetRolesAsync(User user)
		{
			if (user == null)
				throw new NullReferenceException("user");

			return Task.FromResult<IList<string>>(user.Roles.Select(x => x.Name).ToArray());
		}

		public Task<bool> IsInRoleAsync(User user, string roleName)
		{
			if (user == null)
				throw new ArgumentNullException("user");

            try
            {
                return Task.FromResult<bool>(user.Roles.Any(x => x.Name == roleName || x.Id ==roleName));
            }
            catch (System.Exception)
            {
                throw;
            }
		}

		public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.LockoutEndDate);
		}

		public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			user.LockoutEndDate = lockoutEnd;
			return Task.FromResult(user);
		}

		public Task<int> IncrementAccessFailedCountAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			user.FailedCount++;
			return Task.FromResult(user.FailedCount);
		}

		public Task ResetAccessFailedCountAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			user.FailedCount = 0;
			return Task.FromResult(user);
		}

		public Task<int> GetAccessFailedCountAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.FailedCount);
		}

		public Task<bool> GetLockoutEnabledAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult(user.LockoutEnabled);
		}

		public Task SetLockoutEnabledAsync(User user, bool enabled)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			user.LockoutEnabled = enabled;
			return Task.FromResult(user);
		}

		public Task SetTwoFactorEnabledAsync(User user, bool enabled)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			throw new NotImplementedException();
		}

		public Task<bool> GetTwoFactorEnabledAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult(user.TwoFactorEnabled);
		}

		public Task SetPhoneNumberAsync(User user, string phoneNumber)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			user.PhoneNumber = phoneNumber;
			return Task.FromResult(user);
		}

		public Task<string> GetPhoneNumberAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(user.PhoneNumber);
		}

		public Task<bool> GetPhoneNumberConfirmedAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult(true);
		}

		public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			throw new NotImplementedException();
		}

		public Task AddLoginAsync(User user, UserLoginInfo login)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			throw new NotImplementedException();
		}

		public Task RemoveLoginAsync(User user, UserLoginInfo login)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			throw new NotImplementedException();
		}

		public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			return Task.FromResult<IList<UserLoginInfo>>(new List<UserLoginInfo>());
		}

		public Task<User> FindAsync(UserLoginInfo login)
		{
			if (login == null)
				throw new ArgumentNullException("login");

            var loginResult = Session.Query<Login>().Where(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey).FirstOrDefault();
            var user = loginResult == null ? null : loginResult.User;

            return Task.FromResult(user);
		}
	}
}
