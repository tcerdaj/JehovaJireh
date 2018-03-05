using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Logging;
using NLog;
using NLog.Config;
using System.Reflection;
using JehovaJireh.Core.Entities;

namespace JehovaJireh.Infrastructure.Logging
{
	public class NLogLogger : JehovaJireh.Logging.ILogger
	{
		private static Logger log;

		public NLogLogger(LoggingConfiguration config, string loggerName)
		{
			LogManager.Configuration = config;
			if (string.IsNullOrEmpty(loggerName))
			{
				log = LogManager.GetCurrentClassLogger();
			}
			else
			{
				log = LogManager.GetLogger(loggerName);
			}
		}

		public NLogLogger(LoggingConfiguration config)
			: this(config, string.Empty)
		{
		}


		#region Standard Logging
		public void Info(string message)
		{
			log.Info(message);
		}
		public void Info(string fmt, params object[] vars)
		{
			log.Info(fmt, vars);
		}
		public void Info(string message, System.Exception exception, params object[] args)
		{
			log.Info(message, exception, args);
		}

		public void Warn(string message)
		{
			log.Warn(message);
		}
		public void Warn(string fmt, params object[] vars)
		{
			log.Warn(fmt, vars);
		}
		public void Warn(string message, System.Exception exception, params object[] args)
		{
			log.Warn(message, exception, args);
		}

		public void Error(string message)
		{
			log.Error(message);
		}
		public void Error(string fmt, params object[] vars)
		{
			log.Error(fmt, vars);
		}
		public void Error(string message, System.Exception exception, params object[] args)
		{
			log.Error(message, exception, args);
		}

		public void Error(System.Exception exception)
		{
			log.Error(exception);
		}

		public void TraceApi(string componentName, string method, TimeSpan timespan)
		{
			string message = String.Concat("component:", componentName, ";method:", method, ";timespan:", timespan.ToString());
			log.Trace(message);
		}
		public void TraceApi(string componentName, string method, TimeSpan timespan, string properties)
		{
			string message = String.Concat("component:", componentName, ";method:", method, ";timespan:", timespan.ToString(), ";properties:", properties);
			log.Trace(message);
		}
		public void TraceApi(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars)
		{
			string message = String.Concat("component:", componentName, ";method:", method, ";timespan:", timespan.ToString());
			log.Trace(message, fmt, vars);
		}

		public void Debug(string message)
		{
			log.Debug(message);
		}

		public void Debug(string fmt, params object[] vars)
		{
			log.Debug(fmt, vars);
		}

		public void Debug(string message, System.Exception exception, params object[] vars)
		{
			log.Debug(message, exception, vars);
		}

		public void Fatal(string message)
		{
			log.Fatal(message);
		}

		public void Fatal(string message, System.Exception exception, params object[] vars)
		{
			log.Fatal(message, exception, vars);
		}

		public void Fatal(System.Exception exception)
		{
			log.Fatal(exception);
		}

		public void Fatal(string fmt, params object[] vars)
		{
			log.Fatal(fmt, vars);
		}

		#endregion

		#region Application
		public void APIApplicationStarting()
		{
			log.Info("ApiApplication is starting");
		}

		public void APIApplicationStarted()
		{
			log.Info("ApiApplication has started");
		}

		public void WebApplicationStarting()
		{
			log.Info("Web Application is starting");
		}

		public void WebApplicationStarted()
		{
			log.Info("Web Application has started");
		}

		public void WorkerStarted(string workerName)
		{
			log.Info("Worker {0} has started", workerName);
		}

		public void WorkerFinished(string workerName, TimeSpan? span)
		{
			log.Info(string.Format("{0} has finished in {1} seconds", workerName, span.Value.TotalSeconds));
		}
		public void WorkerCancelled(string workerName)
		{
			log.Info("Worker {0} has been cancelled", workerName);
		}

		public System.Exception HandleException(System.Exception exception, Guid handlingInstanceId)
		{
			string message = string.Format("Error: {0}. {1} Handling Instance ID: {2}", exception.ToString(), Environment.NewLine, handlingInstanceId);
			log.Error(message);
			return exception;
		}
		#endregion

		#region Initialization

		public void StopRepositoryInitialized()
		{
			if (log.IsInfoEnabled)
				log.Info("Stop repository is initialized");
		}

		public void UserRepositoryInitialized(string applicationName)
		{
			if (log.IsInfoEnabled)
				log.Info("User repository is initialized");
		}

		public void ProfileStoreInitialized()
		{
			if (log.IsInfoEnabled)
				log.Info("Profile store is initialized");
		}
		#endregion

		#region EntityCRUD operations
		public void GetStarted(string id)
		{
			log.Info("Getting EntityID {0} has started", id);
		}

		public void GetFinished(string id, TimeSpan? span)
		{
			string message = string.Format("Getting EntityID '{0}' has Finished", id);
			message += string.Format(" in {0} seconds", span.Value.TotalSeconds.ToString());
			log.Info(message);
		}

		public void GetStarted<T>(T entity)
		{
            try
            {
                Type entityType = ((object)entity).GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(entityType.GetProperties());
                string info = string.Format("Getting {0} Started entity.{1}", entity.GetType().Name, Environment.NewLine);
                //loop through all the properties in the generic class being updated and log their values
                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(((object)entity), null);
                    info += string.Format("{0}='{1}'{2}", prop.Name, (propValue == null) ? "null" : propValue.ToString(), Environment.NewLine);
                }
                if (log.IsInfoEnabled)
                    log.Info(info);
            }
            catch (System.Exception)
            {

            }
		}

		public void GetFinished<T>(T entity, TimeSpan? span)
		{
            try
            {
                string message = string.Format("Getting Finished {0} entity with ID='{1}'", entity.GetType().Name, ((EntityBase<int>)(object)entity).Id);
                if (span != null)
                {
                    message += string.Format(" in {0} seconds", span.Value.TotalSeconds.ToString());
                }
                if (log.IsInfoEnabled)
                    log.Info(message);
            }
            catch (System.Exception)
            {

            }
           
		}

		public void SaveStarted<T>(T entity)
		{
            try
            {
                Type entityType = ((object)entity).GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(entityType.GetProperties());
                string info = string.Format("Saving Started {0} entity.{1}", entity.GetType().Name, Environment.NewLine);
                //loop through all the properties in the generic class being updated and log their values
                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(((object)entity), null);
                    info += string.Format("{0}='{1}'{2}", prop.Name, (propValue == null) ? "null" : propValue.ToString(), Environment.NewLine);
                }
                if (log.IsInfoEnabled)
                    log.Info(info);
            }
            catch 
            {
            }
           
		}


		public void SaveFinished<T>(T entity, TimeSpan? span)
		{
            try
            {
                string message = string.Format("Saving Finished {0} entity with ID='{1}'", entity.GetType().Name, ((EntityBase<int>)(object)entity).Id);
                if (span != null)
                {
                    message += string.Format(" in {0} seconds", span.Value.TotalSeconds.ToString());
                }
                if (log.IsInfoEnabled)
                    log.Info(message);
            }
            catch (System.Exception)
            {

            }
            
		}

		public void SaveInsertStarted<T>(T entity)
		{

            try
            {
                string info = string.Format("Inserting Started {0} entity.{1}", "Name", "NewLine");
                if(entity != null)
                {
                    Type entityType = ((object)entity).GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(entityType.GetProperties());
                    info = string.Format("Inserting Started {0} entity.{1}", entity.GetType().Name, Environment.NewLine);
                    //loop through all the properties in the generic class being updated and log their values
                    foreach (PropertyInfo prop in props)
                    {
                        object propValue = prop.GetValue(((object)entity), null);
                        info += string.Format("{0}='{1}'{2}", prop.Name, (propValue == null) ? "null" : propValue.ToString(), Environment.NewLine);
                    }
                }
                
                if (log.IsInfoEnabled)
                    log.Info(info);
            }
            catch (System.Exception)
            {

            }
           
		}

		public void SaveInsertFinished<T>(T entity, TimeSpan? span)
		{
            try
            {
                string message = string.Format("Inserting Finished {0} entity with ID='{1}'", "Name", "Id");

                if (entity != null)
                {
                    message = string.Format("Inserting Finished {0} entity with ID='{1}'", entity.GetType().Name, ((EntityBase<int>)(object)entity).Id);
                }

                if (span != null)
                {
                    message += string.Format(" in {0} seconds", span.Value.TotalSeconds.ToString());
                }
                if (log.IsInfoEnabled)
                    log.Info(message);
            }
            catch (System.Exception)
            {

                
            }
            
			
		}

		public void DeleteStarted<T>(T entity)
		{
            try
            {
                string info = string.Format("Deleting {0} entity.{1}", "Name", "NewLine");

                if (entity != null)
                {
                    info = string.Format("Deleting {0} entity.{1}", entity.GetType().Name, Environment.NewLine);
                    Type entityType = ((object)entity).GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(entityType.GetProperties());
                    //loop through all the properties in the generic class being updated and log their values
                    foreach (PropertyInfo prop in props)
                    {
                        object propValue = prop.GetValue(((object)entity), null);
                        info += string.Format("{0}='{1}'{2}", prop.Name, (propValue == null) ? "null" : propValue.ToString(), Environment.NewLine);
                    }
                }

                if (log.IsInfoEnabled)
                    log.Info(info);
            }
            catch (System.Exception)
            {

            }
		}

		public void DeleteFinished<T>(T entity, TimeSpan? span)
		{
            try
            {
                string message = string.Format("Deleting Finished {0} entity with ID='{1}'", "entity", "Id");

                if (entity != null)
                {
                    message = string.Format("Deleting Finished {0} entity with ID='{1}'", entity.GetType().Name, ((EntityBase<int>)(object)entity).Id);
                }
                
                if (span != null)
                {
                    message += string.Format(" in {0} seconds", span.Value.TotalSeconds.ToString());
                }
                if (log.IsInfoEnabled)
                    log.Info(message);
            }
            catch (System.Exception)
            {

                
            }
            
		}

		public void LoginStarted(string username)
		{
			log.Info("Login Started - UserName {0} has started", username);
		}

		public void RegisterStarted<T>(T entity)
		{
			Type entityType = ((object)entity).GetType();
			IList<PropertyInfo> props = new List<PropertyInfo>(entityType.GetProperties());
			string info = string.Format("Register Started  {0} entity.{1}", entity.GetType().Name, Environment.NewLine);
			//loop through all the properties in the generic class being updated and log their values
			foreach (PropertyInfo prop in props)
			{
				object propValue = prop.GetValue(((object)entity), null);
				info += string.Format("{0}='{1}'{2}", prop.Name, (propValue == null) ? "null" : propValue.ToString(), Environment.NewLine);
			}
			//if (log.IsInfoEnabled)
			log.Info(info);
		}

		public void LoginFinished(string userName, string results, TimeSpan? span = null)
		{
			string message = string.Format("Login Finished  - UserName {0} finish Results {1} ", userName, results);
			if (span != null)
			{
				message += string.Format(" in {0} seconds", span.Value.TotalSeconds.ToString());
			}
			//if (log.IsInfoEnabled)
			log.Info(message);
		}

		public void RegisterFinished<T>(T entity, string results, TimeSpan? span = null)
		{
			string message = string.Format("Register Finished {0} entity with ID='{1}', results: {2}", entity.GetType().Name, ((EntityBase<int>)(object)entity).Id, results);
			if (span != null)
			{
				message += string.Format(" in {0} seconds", span.Value.TotalSeconds.ToString());
			}
			if (log.IsInfoEnabled)
				log.Info(message);
		}

		public void LogOff(string userName)
		{
			log.Info(string.Format("LofOff user {0}.", userName));
        }
		#endregion

		#region User Interface

		public void UiSaveExpenseStarted()
		{
		}

		public void UiSaveExpenseFinished()
		{
		}

		public void UiSaveExpenseFailed(string exceptionMessage, string exceptionType)
		{

		}
		#endregion

		#region General

		public void ExceptionHandlerLoggedException(string message, Guid id)
		{

		}

		public void TracingBehaviorVirtualMethodIntercepted(string declaringType, string methodBaseName, string arguments)
		{
		}
		#endregion

		#region Caching
		public void CacheInitialized()
		{
			if (log.IsInfoEnabled)
				log.Info("Cache Initialized");
		}

		public void CacheCleared()
		{
			if (log.IsInfoEnabled)
				log.Info("Cache Cleared");
		}
		#endregion

		public bool IsDebugEnabled
		{
			get
			{
				return log.IsDebugEnabled;
			}
		}

		public bool IsErrorEnabled
		{
			get
			{
				return log.IsErrorEnabled;
			}
		}

		public bool IsFatalEnabled
		{
			get
			{
				return log.IsFatalEnabled;
			}
		}

		public bool IsInfoEnabled
		{
			get
			{
				return log.IsInfoEnabled;
			}
		}

		public bool IsWarnEnabled
		{
			get
			{
				return log.IsWarnEnabled;
			}
		}

		public static NLog.Config.LoggingConfiguration GetConfiguration()
		{
			NLog.Config.ConfigurationItemFactory.Default.Targets.RegisterDefinition("AzureTableStorageTarget", typeof(NLog.Extensions.AzureTableStorage.AzureTableStorageTarget));
			NLog.Config.ConfigurationItemFactory.Default.Targets.RegisterDefinition("AzureTableStorageTargetCustom", typeof(NLogAzureStorageTarget));
			NLog.Config.ConfigurationItemFactory.Default.Targets.RegisterDefinition("AzureTableStorageTargetCustom", typeof(NLog.Targets.ConsoleTarget));

			NLog.Config.LoggingConfiguration config = new NLog.Config.LoggingConfiguration();

			var targetAzure = new NLog.Extensions.AzureTableStorage.AzureTableStorageTarget();
			targetAzure.Layout = "${json-encode:jsonEncode=True:inner=${message}}";
			targetAzure.Name = "tableStorage";
			targetAzure.ConnectionString = JehovaJireh.Configuration.CloudConfiguration.GetConnectionString("LogStorageConnectionString");
			targetAzure.TableName = "logging";
			config.AddTarget("tableStorage", targetAzure);

			var targetAzureCustom = new NLogAzureStorageTarget();
			//targetAzureCustom.Layout = "${json-encode:jsonEncode=True:inner=${message}}";
			targetAzureCustom.Name = "tableStorageCustom";
			targetAzureCustom.TableStorageConnectionStringName = "LogStorageConnectionString";
			targetAzureCustom.TableName = "logging";
			config.AddTarget("tableStorageCustom", targetAzure);

			var targetConsole = new NLog.Targets.ConsoleTarget();
			targetConsole.Layout = "${json-encode:jsonEncode=True:inner=${message}}";
			targetConsole.Name = "console";
			config.AddTarget("console", targetConsole);

			var ruleNHibernate = new NLog.Config.LoggingRule("NHibernate*", NLog.LogLevel.Debug, targetAzureCustom);
			var ruleApplication = new NLog.Config.LoggingRule("JehovaJireh*", NLog.LogLevel.Trace, targetAzureCustom);
			config.LoggingRules.Add(ruleNHibernate);
			config.LoggingRules.Add(ruleApplication);

			var ruleConsole = new NLog.Config.LoggingRule("*", NLog.LogLevel.Trace, targetConsole);
			config.LoggingRules.Add(ruleConsole);
			return config;
		}
	}
}
