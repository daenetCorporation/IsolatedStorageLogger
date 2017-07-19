
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Daenet.Common.Logging.IsolatedStorageLogger;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using Daenet.Common.Logging.IsolatedStorageLogger;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private ILogger m_Logger;
        public UnitTest1()
        {
            init(null);
        }

        [TestMethod]
        public void LogsAllTypesNoFilter()
        {
            m_Logger.LogTrace("Test Trace Log Message");
            m_Logger.LogDebug("Test Debug Log Message");
            m_Logger.LogInformation("Test Information Log Message");
            m_Logger.LogWarning("Test Warning Log Message");
            m_Logger.LogError(new EventId(456, "txt456"), "456 Test Error Log Message");
            m_Logger.LogCritical(new EventId(123, "txt123"), "123 Test Critical Log Message");
        }

        private void init(Func<string, LogLevel, bool> filter,
              Func<LogLevel, EventId, object, Exception, string> eventDataFormatter = null,
           Dictionary<string, object> additionalValues = null)
        {
            Dictionary<string, LogLevel> dictionary = new Dictionary<string, LogLevel>();
            dictionary.Add("UnitTests", 0);

            var setting = new IsolatedStorageLoggerSettings()
            {
                Directory = "IsolatedStrorageLog",
                FileName = "log.json",
                Switches = dictionary
            };

            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddIsolatedStorage(setting, filter, eventDataFormatter, additionalValues);

            m_Logger = loggerFactory.CreateLogger<UnitTest1>();
        }
    }
}
