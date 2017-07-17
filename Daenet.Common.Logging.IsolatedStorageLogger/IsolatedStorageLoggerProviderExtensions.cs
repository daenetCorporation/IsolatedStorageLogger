using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    public static class IsolatedStorageLoggerProviderExtensions
    {
        public static IIsolatedStorageLoggerSettings GetIsolatedStorageLoggerSettings(this IConfiguration config)
        {
            var setting = new IsolatedStorageLoggerSettings();
            setting.Directory = config.GetSection("IsolatedStorage").GetValue<string>("Directory");
            setting.FileName = config.GetSection("IsolatedStorage").GetValue<string>("FileName");

            config.GetSection("Switches").Bind(setting.Switches);

            return setting;
        }
    }
}
