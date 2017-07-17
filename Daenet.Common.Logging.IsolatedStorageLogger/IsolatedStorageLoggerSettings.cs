using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    class IsolatedStorageLoggerSettings : IIsolatedStorageLoggerSettings
    {
        public IDictionary<string, LogLevel> Switches { get; set; } = new Dictionary<string, LogLevel>();
        public string Directory { get; set; }
        public string FileName { get; set; }

        public bool TryGetSwitch(string name, out LogLevel level)
        {
            return Switches.TryGetValue(name, out level);
        }
    }
}
