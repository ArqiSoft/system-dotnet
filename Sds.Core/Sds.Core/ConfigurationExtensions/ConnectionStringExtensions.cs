using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Sds.Core
{
    public static class ConnectionStringSettingsExtensions
    {
        public static int Timeout(this ConnectionStringSettings connection, int def = 60)
        {
            var match = Regex.Match(connection.ConnectionString, @"Connection Timeout=(?<timeout>\d+)");

            return match.Success ? Convert.ToInt32(match.Groups["timeout"].Value) : def;
        }
    }
}
