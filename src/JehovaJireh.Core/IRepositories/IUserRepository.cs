using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Core.Entities;
using JehovaJireh.Logging;

namespace JehovaJireh.Core.IRepositories
{
	public interface IUserRepository:IRepository<User, int>
	{
		User GetByUserName(string userName);
		User GetByEmail(string email);
		User GetByConfirmationToken(string token);
	}
}
