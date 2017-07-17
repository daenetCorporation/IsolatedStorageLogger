
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Daenet.Common.Logging.IsolatedStorageLogger;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void WriteFile()
        {

        }

        private void getIsolatedStorageSettings()
        {
            ConfigurationBuilder cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddJsonFile(@"IsolatedStorageSettings.json");
            var cfgRoot = cfgBuilder.Build();
            cfgRoot.GetIsolatedStorageLoggerSettings();

        }
    }
}
