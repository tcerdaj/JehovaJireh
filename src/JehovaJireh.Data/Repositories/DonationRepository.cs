using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Core.Entities;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NHibernate;

namespace JehovaJireh.Data.Repositories
{
	public class DonationRepository:NHRepository<Donation,int>, IDonationRepository
	{
		public DonationRepository(ISession session, ExceptionManager exManager, ILogger log)
			:base(session, exManager, log)
		{

		}
	}
}
