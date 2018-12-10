using Serilog.Core;
using Serilog.Events;
using System;

namespace Sds.Serilog
{
    public class EnvironmentVariableLoggingLevelSwitch : LoggingLevelSwitch
    {
        public EnvironmentVariableLoggingLevelSwitch(string environmentVariable)
        {
            LogEventLevel level = LogEventLevel.Information;
            if (Enum.TryParse<LogEventLevel>(Environment.ExpandEnvironmentVariables(environmentVariable), true, out level))
            {
                MinimumLevel = level;
            }
        }
    }
}
