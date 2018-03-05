using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using JehovaJireh.Configuration;
using JehovaJireh.Data.Repositories;
using Microsoft.WindowsAzure.Storage.Table;
using NLog;
using NLog.Targets;

namespace JehovaJireh.Infrastructure.Logging
{
	[Target("AzureStorage")]
	public class NLogAzureStorageTarget:Target
	{
		private ATSRepository<LogEntry> logRepository;
		private string applicationName, roleInstance, deploymentId;
		//[Required]
		public string TableStorageConnectionStringName { get; set; }
		public string TableName { get; set; }
		protected override void InitializeTarget()
		{
			if (string.IsNullOrEmpty(TableName))
			{
				throw new ArgumentNullException("TableName");
			}

			base.InitializeTarget();
			var storageAccount = CloudConfiguration.GetStorageAccount("LogStorageConnectionString");
			logRepository = new ATSRepository<LogEntry>(storageAccount.CreateCloudTableClient(), TableName);

			try
			{
				applicationName = BuildManager.GetGlobalAsaxType().BaseType.Assembly.GetName().Name;
			}
			catch
			{
				applicationName = Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.CurrentRoleInstance.Role.Name;
			}

			try
			{
				roleInstance = Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.CurrentRoleInstance.Id;
			}
			catch
			{
				roleInstance = "N/A";

			}

			try
			{
				deploymentId = Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.DeploymentId;
			}
			catch
			{
				deploymentId = "N/A";
			}
		}
		protected override void Write(LogEventInfo loggingEvent)
		{
			Action doWriteToLog = () =>
			{
				//try
				//{                                   
				logRepository.Insert(new LogEntry
				{

					RoleInstance = roleInstance,
					DeploymentId = deploymentId,
					Application = applicationName,
					Timestamp = loggingEvent.TimeStamp,
					Message = loggingEvent.FormattedMessage,
					Level = loggingEvent.Level.Name,
					LoggerName = loggingEvent.LoggerName,
					StackTrace = loggingEvent.StackTrace != null ? loggingEvent.StackTrace.ToString() : null
				});
				//}
				//catch (DataServiceRequestException e)
				//{
				//    InternalLogger.Error(string.Format("{0}: Could not write log entry to {1}: {2}",
				//        GetType().AssemblyQualifiedName, _tableEndpoint, e.Message), e);
				//}
			};
			doWriteToLog.BeginInvoke(null, null);
		}
	}

	public class LogEntry : TableEntity
	{
		public LogEntry()
		{
			var now = DateTime.UtcNow;
			PartitionKey = string.Format("{0:yyyy-MM}", now);
			RowKey = string.Format("{0:dd HH:mm:ss.fff}-{1}", now, Guid.NewGuid());
		}
		#region Table columns
		public string Message { get; set; }
		public string Level { get; set; }
		public string LoggerName { get; set; }
		public string RoleInstance { get; set; }
		public string DeploymentId { get; set; }
		public string Application { get; set; }
		public string StackTrace { get; set; }
		#endregion
	}
}
