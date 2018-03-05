using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Logging;
using NHibernate;

namespace JehovaJireh.Infrastructure.Logging
{
	public class NLogNHibernateFactory : ILoggerFactory
	{
		#region ILoggerFactory Members

		public IInternalLogger LoggerFor(System.Type type)
		{
			ILogger logger = new Logging.NLogLogger(Logging.NLogLogger.GetConfiguration(), "NHibernate");
			return new NLogNHibernateLogger(logger);
		}

		public IInternalLogger LoggerFor(string keyName)
		{
			ILogger logger = new Logging.NLogLogger(Logging.NLogLogger.GetConfiguration(), "NHibernate");
			return new NLogNHibernateLogger(logger);
		}

		#endregion
	}

	public class NLogNHibernateLogger : IInternalLogger
	{
		private readonly ILogger logger;

		public NLogNHibernateLogger(ILogger logger)
		{
			this.logger = logger;
			IsDebugEnabled = logger.IsDebugEnabled;
			IsErrorEnabled = logger.IsErrorEnabled;
			IsFatalEnabled = logger.IsFatalEnabled;
			IsInfoEnabled = logger.IsInfoEnabled;
			IsWarnEnabled = logger.IsWarnEnabled;
		}

		#region IInternalLogger Members

		#region Properties

		public bool IsDebugEnabled { get; private set; }

		public bool IsErrorEnabled { get; private set; }

		public bool IsFatalEnabled { get; private set; }

		public bool IsInfoEnabled { get; private set; }

		public bool IsWarnEnabled { get; private set; }

		#endregion

		#region IInternalLogger Methods

		public void Debug(object message, System.Exception exception)
		{
			if (IsDebugEnabled)
				logger.Debug(message.ToString(), exception);
		}

		public void Debug(object message)
		{
			if (IsDebugEnabled)
				logger.Debug(message.ToString());
		}

		public void DebugFormat(string format, params object[] args)
		{
			if (IsDebugEnabled)
				logger.Debug(String.Format(format, args));
		}

		public void Error(object message, System.Exception exception)
		{
			if (IsErrorEnabled)
				logger.Error(message.ToString(), exception);
		}

		public void Error(object message)
		{
			if (IsErrorEnabled)
				logger.Error(message.ToString());
		}

		public void ErrorFormat(string format, params object[] args)
		{
			if (IsErrorEnabled)
				logger.Error(String.Format(format, args));
		}

		public void Fatal(object message, System.Exception exception)
		{
			if (IsFatalEnabled)
				logger.Fatal(message.ToString(), exception);
		}

		public void Fatal(object message)
		{
			if (IsFatalEnabled)
				logger.Fatal(message.ToString());
		}

		public void Info(object message, System.Exception exception)
		{
			if (IsInfoEnabled)
				logger.Info(message.ToString(), exception);
		}

		public void Info(object message)
		{
			if (IsInfoEnabled)
				logger.Info(message.ToString());
		}

		public void InfoFormat(string format, params object[] args)
		{
			if (IsInfoEnabled)
				logger.Info(String.Format(format, args));
		}

		public void Warn(object message, System.Exception exception)
		{
			if (IsWarnEnabled)
				logger.Warn(message.ToString(), exception);
		}

		public void Warn(object message)
		{
			if (IsWarnEnabled)
				logger.Warn(message.ToString());
		}

		public void WarnFormat(string format, params object[] args)
		{
			if (IsWarnEnabled)
				logger.Warn(String.Format(format, args));
		}

		#endregion
		#endregion
	}
}
