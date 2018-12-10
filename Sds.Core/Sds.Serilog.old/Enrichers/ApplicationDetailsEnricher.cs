using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Reflection;

namespace Sds.Serilog
{
    public class ApplicationDetailsEnricher : ILogEventEnricher
    {
        private Assembly GetEntryAssembly()
        {
            if (System.Web.HttpContext.Current == null ||
                System.Web.HttpContext.Current.ApplicationInstance == null)
            {
                return Assembly.GetEntryAssembly();
            }

            var type = System.Web.HttpContext.Current.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            return type == null ? null : type.Assembly;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var assembly = GetEntryAssembly();

            if (assembly != null)
            {
                var name = assembly.GetName().Name;
                var ver = assembly.GetName().Version;

                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ApplicationName", name));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ApplicationVersion", ver));
            }
        }
    }

    public static class ApplicationDetailsEnricherExtensions
    {
        public static LoggerConfiguration WithApplicationDetails(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException("enrichmentConfiguration");
            }
            return enrichmentConfiguration.With<ApplicationDetailsEnricher>();
        }
    }
}
