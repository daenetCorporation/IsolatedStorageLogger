using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    internal class IsolatedStorageLogScopeManager
    {
        public IsolatedStorageLogScopeManager(object state)
        {

        }

        public object Current { get; internal set; }

        internal IDisposable Push(object state)
        {
            throw new NotImplementedException();
        }
    }
}
