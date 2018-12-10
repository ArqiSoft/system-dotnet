using Sds.MassTransit.Settings;
using System;
using System.Collections.Specialized;

namespace Sds.MassTransit.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static MassTransitSettings MassTransitSettings(this NameValueCollection settings)
        {
            var mtSettings = new MassTransitSettings();

            if (!string.IsNullOrEmpty(settings["MassTransit:ConnectionString"]))
                mtSettings.ConnectionString = settings["MassTransit:ConnectionString"];

            if (!string.IsNullOrEmpty(settings["MassTransit:PrefetchCount"]))
                mtSettings.PrefetchCount = Convert.ToUInt16(settings["MassTransit:PrefetchCount"]);

            if (!string.IsNullOrEmpty(settings["MassTransit:ConcurrencyLimit"]))
                mtSettings.ConcurrencyLimit = Convert.ToInt32(settings["MassTransit:ConcurrencyLimit"]);

            return mtSettings;
        }

        public static TestHarnessSettings TestHarnessSettings(this NameValueCollection settings)
        {
            var harnessSettings = new TestHarnessSettings();

            if (!string.IsNullOrEmpty(settings["MassTransit:TestHarnessTimeout"]))
                harnessSettings.Timeout = Convert.ToInt32(settings["MassTransit:TestHarnessTimeout"]);

            return harnessSettings;
        }
    }
}
