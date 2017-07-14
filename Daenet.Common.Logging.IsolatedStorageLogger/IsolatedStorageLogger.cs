using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    public class IsolatedStorageLogger : ILogger
    {
        private IsolatedStorageLogScopeManager m_ScopeManager;
        private Func<string, LogLevel, bool> m_Filter;
        private string m_CategoryName;

        public IDisposable BeginScope<TState>(TState state)
        {
            if(state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if(m_ScopeManager == null)
            {
                m_ScopeManager = new IsolatedStorageLogScopeManager(state);
            }

            var scope = m_ScopeManager.Push(state);

            return scope;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return m_Filter(m_CategoryName, logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
