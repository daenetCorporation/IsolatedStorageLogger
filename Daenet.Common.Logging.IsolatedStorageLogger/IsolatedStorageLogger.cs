using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;
using System.IO;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    public class IsolatedStorageLogger : ILogger
    {
        #region Member Variables
        private IsolatedStorageLogScopeManager m_ScopeManager;
        private Func<string, LogLevel, bool> m_Filter;
        private string m_CategoryName;
        private Dictionary<string, object> m_AdditionalValues;
        private IsolatedStorageFile m_IsolatedStorageFile;
        private IIsolatedStorageLoggerSettings m_Settings;
        #endregion

        #region Properties
        public Func<LogLevel, EventId, object, Exception, string> IsolatedDataFormatter { get; set; } 
        #endregion

        #region Public Methods
        public IsolatedStorageLogger(IIsolatedStorageLoggerSettings settings,
            string categoryName, Func<string, LogLevel, bool> filter = null,
            Func<LogLevel, EventId, object, Exception, string> isolatedDataFormatter = null,
            Dictionary<string, object> additionalValues = null)
        {
            if (filter == null)
                m_Filter = filter ?? ((category, logLevel) => true);
            else
                m_Filter = filter;

            this.m_AdditionalValues = additionalValues;

            m_Settings = settings;

            m_CategoryName = categoryName;

            IsolatedDataFormatter = isolatedDataFormatter == null ? defaultIsolatedDataFormatter : isolatedDataFormatter;

            m_IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            
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

            string isoData = IsolatedDataFormatter(logLevel, eventId, state, exception);

            WriteFile(m_Settings.Directory, m_Settings.FileName, isoData, m_IsolatedStorageFile);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Write message to specified file 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="message"></param>
        /// <param name="isf"></param>
        private void WriteFile(string path, string fileName,string message, IsolatedStorageFile isf)
        {
            if (isf.DirectoryExists(path))
                isf.CreateDirectory(path);

            if(!String.IsNullOrEmpty(message))
            {
                using (IsolatedStorageFileStream fs = new IsolatedStorageFileStream($@"{path}\{fileName}", System.IO.FileMode.Append, isf))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(message);
                    }
                }
            }

        }

        /// <summary>
        /// Write foler structure
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isolatedStorageFile"></param>
        private void WriteDirectory(string path, IsolatedStorageFile isolatedStorageFile)
        {
            if (!isolatedStorageFile.DirectoryExists(path))
                isolatedStorageFile.CreateDirectory(path);
        }

        /// <summary>
        /// Formatte data for Isolated storage
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
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
