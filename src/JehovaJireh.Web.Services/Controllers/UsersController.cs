using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JehovaJireh.Core.Entities;
using JehovaJireh.Core.EntitiesDto;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Logging;

namespace JehovaJireh.Web.Services.Controllers
{
    public class UsersController : BaseController<User,UserDto,int>
    {

		public UsersController(IUserRepository repository, ILogger log)
			:base(repository, log)
		{

		}
    }
}
