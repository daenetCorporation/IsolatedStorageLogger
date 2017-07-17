using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using Microsoft.Extensions.Logging;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    public interface IIsolatedStorageLoggerSettings
    {
        /// <summary>
        /// Folder stacture
        /// </summary>
        string Directory { get; set; }
        /// <summary>
        /// File name
        /// </summary>
        string FileName { get; set; }
        bool TryGetSwitch(string name, out LogLevel level);
    }
}
