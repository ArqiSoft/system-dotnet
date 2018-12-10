using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Sds.Core
{
    public static class ConfigurationExtensions
	{
		/// <summary>
		/// Tries to get and return a value from NameValueCollection by a key; if nothing is found returns a default value
		/// </summary>
		/// <param name="config">NameValueCollection</param>
		/// <param name="settingKey">Settting Key</param>
		/// <param name="defaultValue">Default Value</param>
		/// <returns></returns>
		public static bool ReadBool(this NameValueCollection config, string settingKey, bool defaultValue = default(bool))
		{
			bool value = defaultValue;

			if (bool.TryParse(config[settingKey], out value))
				return value;

			return defaultValue;
		}

		/// <summary>
		/// Tries to get and return a value from NameValueCollection by a key; if nothing is found returns a default value
		/// </summary>
		/// <param name="config">NameValueCollection</param>
		/// <param name="settingKey">Settting Key</param>
		/// <param name="defaultValue">Default Value</param>
		/// <returns></returns>
		public static short ReadShort(this NameValueCollection config, string settingKey, short defaultValue = default(short))
		{
			short value = defaultValue;

			if (short.TryParse(config[settingKey], out value))
				return value;

			return defaultValue;
		}

		/// <summary>
		/// Tries to get and return a value from NameValueCollection by a key; if nothing is found returns a default value
		/// </summary>
		/// <param name="config">NameValueCollection</param>
		/// <param name="settingKey">Settting Key</param>
		/// <param name="defaultValue">Default Value</param>
		/// <returns></returns>
		public static int ReadInt(this NameValueCollection config, string settingKey, int defaultValue = default(int))
		{
			int value = defaultValue;

			if (int.TryParse(config[settingKey], out value))
				return value;

			return defaultValue;
		}

		/// <summary>
		/// Tries to get and return a value from NameValueCollection by a key; if nothing is found returns a default value
		/// </summary>
		/// <param name="config">NameValueCollection</param>
		/// <param name="settingKey">Settting Key</param>
		/// <param name="defaultValue">Default Value</param>
		/// <returns></returns>
		public static long ReadLong(this NameValueCollection config, string settingKey, long defaultValue = default(long))
		{
			long value = defaultValue;

			if (long.TryParse(config[settingKey], out value))
				return value;

			return defaultValue;
		}

		/// <summary>
		/// Tries to get and return a value from NameValueCollection by a key; if nothing is found returns a default value
		/// </summary>
		/// <param name="config">NameValueCollection</param>
		/// <param name="settingKey">Settting Key</param>
		/// <param name="defaultValue">Default Value</param>
		/// <returns></returns>
		public static string ReadString(this NameValueCollection config, string settingKey, string defaultValue = null)
		{
			string value = config[settingKey];

			if (!string.IsNullOrEmpty(value))
				return value;

			return defaultValue;
		}

		/// <summary>
		/// Expands configuration settings from environment variable and retruns new NameValueCollection
		/// </summary>
		/// <param name="config">Name-value configuration section</param>
		/// <returns>new name-value, expanded from environment valirables</returns>
		public static NameValueCollection ExpandFromEnvironmentVariables(this NameValueCollection config)
		{
			NameValueCollection expanded = new NameValueCollection();
			foreach (var key in config.AllKeys)
			{
				expanded[key] = Environment.ExpandEnvironmentVariables(key);
			}
			return expanded;
		}
	}
}
