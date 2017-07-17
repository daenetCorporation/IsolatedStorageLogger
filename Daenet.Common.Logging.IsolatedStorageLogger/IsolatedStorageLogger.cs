using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    public class IsolatedStorageLogger : ILogger
    {
        #region Member Variables
        private IsolatedStorageLogScopeManager m_ScopeManager;
        private Func<string, LogLevel, bool> m_Filter;
        private string m_CategoryName;
        private Dictionary<string, object> m_AdditionalValues;
        #endregion

        #region Properties
        public Func<LogLevel, EventId, object, Exception, string> IsolatedDataFormatter { get; set; } 
        #endregion

        #region Public Methods
        public IsolatedStorageLogger()
        {

        }
        public IDisposable BeginScope<TState>(TState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (m_ScopeManager == null)
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
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string iData = IsolatedDataFormatter(logLevel, eventId, state, exception);

            //Write file to Isolated Strage 
        } 
        #endregion

        #region Private Methods
        private string defaultIsolatedDataFormatter(LogLevel logLevel, EventId eventId, object state, Exception exception)
        {
            System.Dynamic.ExpandoObject expando = new System.Dynamic.ExpandoObject();
            var data = (System.Collections.Generic.IDictionary<String, Object>)expando;

            data.Add("Name", m_CategoryName);
            data.Add("Scope", m_ScopeManager == null ? null : m_ScopeManager.Current);
            data.Add("EventId", eventId.ToString());
            data.Add("Message", state.ToString());
            data.Add("Level", logLevel);
            data.Add("LocalEnqueuedTime", DateTime.Now.ToString("O"));
            data.Add("Exception", exception == null ? null : new
            {
                Message = exception.Message,
                Type = exception.GetType().Name,
                StackTrace = exception.StackTrace
            });

            if (this.m_AdditionalValues != null)
            {
                foreach (var item in this.m_AdditionalValues)
                {
                    data.Add(item.Key, item.Value);
                }
            }

            var payload = JsonConvert.SerializeObject(data);

            System.Diagnostics.Debug.Write(payload);

            return payload;
        } 
        #endregion
    }
}
