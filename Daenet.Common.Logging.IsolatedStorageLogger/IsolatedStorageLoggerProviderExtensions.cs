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
            return new IsolatedStorageLoggerSettings()
            {
                Directory = config.GetValue<string>("Directory"),
                FileName = config.GetValue<string>("FileName")
        };

        }
    }
}
