using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    class IsolatedStorageLoggerProvider : ILoggerProvider
    {
        #region Member Variables
        private readonly ConcurrentDictionary<string, IsolatedStorageLogger> m_Loggers = new ConcurrentDictionary<string, IsolatedStorageLogger>();
        private Dictionary<string, object> m_AdditionalValues;
        private Func<string, LogLevel, bool> m_Filter;
        private IIsolatedStorageLoggerSettings m_Settings;
        private Func<LogLevel, EventId, object, Exception, string> m_IsolatedDataFormatter;
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="filter"></param>
        /// <param name="isolatedDataFormatter"></param>
        /// <param name="additionalValues"></param>
        public IsolatedStorageLoggerProvider(IIsolatedStorageLoggerSettings settings, Func<string, LogLevel, bool> filter, Func<LogLevel, EventId, object, Exception, string> isolatedDataFormatter = null, Dictionary<string, object> additionalValues = null)
        {
            this.m_IsolatedDataFormatter = isolatedDataFormatter;
            this.m_AdditionalValues = additionalValues;
            this.m_Filter = filter;
            this.m_Settings = settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return createLoggerImplementation(categoryName);
        }

        public void Dispose()
        {
            
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        private IsolatedStorageLogger createLoggerImplementation(string categoryName)
        {
            return new IsolatedStorageLogger(m_Settings, categoryName, getFilter(categoryName, m_Settings), m_IsolatedDataFormatter, m_AdditionalValues);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private Func<string, LogLevel, bool> getFilter(string name, IIsolatedStorageLoggerSettings settings)
        {
            if (m_Filter != null)
            {
                return m_Filter;
            }

            if (settings != null)
            {
                foreach (var prefix in getKeyPrefixes(name))
                {
                    LogLevel level;
                    if (settings.TryGetSwitch(prefix, out level))
                    {
                        return (n, l) => l >= level;
                    }
                }
            }

            return (n, l) =>
            false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IEnumerable<string> getKeyPrefixes(string name)
        {
            List<string> names = new List<string>();

            var tokens = name.Split('.');

            names.Add(name);

            string currName = name;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < tokens.Length - 1; i++)
            {
                sb.Append(tokens[i]);
                names.Add(sb.ToString());
                if (i < tokens.Length - 1)
                    sb.Append(".");
            }

            names.Add("Default");

            return names;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IEnumerable<string> getKeyPrefix(string name)
        {
            List<string> names = new List<string>();

            var tokens = name.Split('.');

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < tokens.Length - 1; i++)
            {
                sb.Append(tokens[i]);
                names.Add(sb.ToString());
                if (i < tokens.Length - 1)
                    sb.Append(".");
            }

            names.Add("Default");

            return names;
        }
        #endregion
    }
}
