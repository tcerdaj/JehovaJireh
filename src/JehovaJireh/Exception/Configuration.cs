using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NHibernate;

namespace JehovaJireh.Exception
{
	public class Configuration
	{
		public List<ExceptionPolicyDefinition> getPolicies(JehovaJireh.Logging.ILogger log)
		{
			var policies = new List<ExceptionPolicyDefinition>();

			#region Data Access Exceptions
			var webServicePolicy = new List<ExceptionPolicyEntry>
			  {
                  {
                     //SQL Exception
                      new ExceptionPolicyEntry(typeof(SqlException),
                          PostHandlingAction.ThrowNewException,
                          new IExceptionHandler[]
                             {
                               new WrapHandler(
                                   "An SQL error occurred and has been logged." +
                                   "Please contact your administrator. " + "SqlException",
                               typeof(SqlException)),
                               log
                             })
                     },
                     {
                        //Timeout Exception
                         new ExceptionPolicyEntry(typeof(TimeoutException),
                          PostHandlingAction.ThrowNewException,
                          new IExceptionHandler[]
                             {
                                 new WrapHandler(
                                   "An Time out error occurred and has been logged." +
                                   "Please contact your administrator. " + "TimeoutException",
                               typeof(TimeoutException)),
                               log
                         })
                     },
                     {
                       //Server Exception
                        new ExceptionPolicyEntry(typeof (ServerException),
                         PostHandlingAction.ThrowNewException,
                         new IExceptionHandler[]
                             {
                               new WrapHandler(
                                   "An server error occurred and has been logged." +
                                   "Please contact your administrator. " + "ServerException" ,
                               typeof(ServerException)),
                               log
                           })
                     },
                     {
                        new ExceptionPolicyEntry(typeof (InvalidOperationException),
                         PostHandlingAction.ThrowNewException,
                         new IExceptionHandler[]
                             {
                               new WrapHandler(
                                   "An server error occurred and has been logged." +
                                   "Please contact your administrator. " + "InvalidOperationException",
                               typeof(InvalidOperationException)),
                               log
                           })
                     },
                     {
                        new ExceptionPolicyEntry(typeof (HibernateException),
                         PostHandlingAction.ThrowNewException,
                         new IExceptionHandler[]
                             {
                               new WrapHandler(
                                   "An server error occurred and has been logged." +
                                   "Please contact your administrator. " + "HibernateException",
                               typeof(HibernateException)),
                               log
                           })
                     }
                };

			#endregion

			policies.Add(new ExceptionPolicyDefinition(ExceptionPolicies.DataAccess.WebServicePolicy, webServicePolicy));

			return policies;
		}
	}
}
