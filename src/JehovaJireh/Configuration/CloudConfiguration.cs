using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace JehovaJireh.Configuration
{
	public static class CloudConfiguration
	{
		public static CloudStorageAccount GetStorageAccount(string settingName)
		{
			var connString = CloudConfigurationManager.GetSetting(settingName);
			return CloudStorageAccount.Parse(connString);
		}

		public static string GetConnectionString(string settingName)
		{
			// Get the connection string from the service configuration file.
			// If it fails, will look for the setting in the appSettings section of the web.config
			var connString = CloudConfigurationManager.GetSetting(settingName);

			if (string.IsNullOrWhiteSpace(connString))
			{
				// Fall back to the connectionStrings section of the web.config
				return ConfigurationManager.ConnectionStrings[settingName].ConnectionString;
			}

			return connString;
		}

		public static string GetSetting(string settingName)
		{
			// Get the setting from the service configuration file.
			var connString = CloudConfigurationManager.GetSetting(settingName);
			return connString;
		}
	}
}
