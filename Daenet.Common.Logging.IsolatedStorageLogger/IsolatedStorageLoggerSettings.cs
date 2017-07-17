using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    class IsolatedStorageLoggerSettings : IIsolatedStorageLoggerSettings
    {
        public string Directory { get; set; }
        public string FileName { get; set; }
    }
}
