using JehovaJireh.Core.Entities;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Logging;
using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace JehovaJireh.Data.Repositories
{
    public class RoleRepository : NHRepository<Role, string>, IRoleRepository, IRoleStore<Role>
    {
        ISession session;
        ILogger log;
        
        public RoleRepository(ISession session, ExceptionManager exManager, ILogger log)
            : base(session, exManager, log)
        {
            this.log = log;
        }
        public RoleRepository(ISession session)
            : base(session)
        {

        }

        public RoleRepository()
        {

        }

        public Task CreateAsync(Role role)
        {
            if ((object)role == null)
                throw new ArgumentNullException("role");

            try
            {
                this.Create(role);
            }
            catch (System.Exception)
            {
                throw;
            }

            return Task.FromResult(role);
        }

        public Task DeleteAsync(Role role)
        {
            if ((object)role == null)
                throw new ArgumentNullException("role");

            try
            {
                this.Delete(role);
            }
            catch (System.Exception)
            {
                throw;
            }

            return Task.FromResult(role);
        }

        public Task<Role> FindByIdAsync(string roleId)
        {
            if (roleId == null)
                throw new ArgumentNullException("roleId");

            try
            {
                var role = (from r in this.Query() where r.Id == roleId select r).FirstOrDefault();
                return Task.FromResult(role);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            if (roleName == null)
                throw new ArgumentNullException("roleName");

            try
            {
                var role = (from r in this.Query() where r.Name == roleName select r).FirstOrDefault();
                return Task.FromResult(role);
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        public List<Role> GetRolesByUserId(string userId)
        {
            if (userId == null)
                throw new ArgumentNullException("userId");

            try
            {
                var user = session.Query<User>().Where(x => x.Id == int.Parse(userId)).FirstOrDefault();
                var roles = user != null ? user.Roles.ToList<Role>() : null;
                return roles;
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        public Task UpdateAsync(Role role)
        {
            if ((object)role == null)
                throw new ArgumentNullException("role");

            try
            {
                this.Update(role);
            }
            catch (System.Exception ex)
            {
                throw;
            }

            return Task.FromResult(role);
        }
        
    }
}
