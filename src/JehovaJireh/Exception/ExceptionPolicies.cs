using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace JehovaJireh.Exception
{
	public static class ExceptionPolicies
	{
		public static class DataAccess
		{
			public const string WebServicePolicy = "WebServicePolicy";
		}

		public static ExceptionManager AssigPolicyToExManager(JehovaJireh.Logging.ILogger log)
		{
			Configuration eConfig = new Configuration();
			var policies = eConfig.getPolicies(log);
			return new ExceptionManager(policies); ;
		}
	}
}
