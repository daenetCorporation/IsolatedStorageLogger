﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    public static class IsolatedStorageLoggerProviderExtensions
    {

        public static ILoggerFactory AddIsolatedStorage(this ILoggerFactory loggerFactory, IIsolatedStorageLoggerSettings settings,
            Func<string, LogLevel, bool> filter = null, Func<LogLevel, EventId, object, Exception, string> isolatedStorageDataFormatter = null,
            Dictionary<string, object> additionalValues = null, string providerName = "")
        {
            loggerFactory.AddProvider(new IsolatedStorageLoggerProvider(settings, filter, isolatedStorageDataFormatter, additionalValues));

            return loggerFactory;
        }

        public static IIsolatedStorageLoggerSettings GetIsolatedStorageLoggerSettings(this IConfiguration config)
        {
            var setting = new IsolatedStorageLoggerSettings();
            setting.Directory = config.GetSection("IsolatedStorage").GetValue<string>("Directory");
            setting.FileName = config.GetSection("IsolatedStorage").GetValue<string>("FileName");

            config.GetSection("Switches").Bind(setting.Switches);

            return setting;
        }

        public static IIsolatedStorageLoggerSettings GetIsolatedStorageLoggerSettings(string path)
        {
            if (Directory.Exists(path))
            {
                using (FileStream fstr = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fstr))
                    {
                        var text = reader.ReadToEnd();
                        
                    }
                }
            }
            else
            {
                throw new FileNotFoundException($"{path} is not found!");
            }
            //var setting = new IsolatedStorageLoggerSettings();
            //setting.Directory = config.GetSection("IsolatedStorage").GetValue<string>("Directory");
            //setting.FileName = config.GetSection("IsolatedStorage").GetValue<string>("FileName");

            //config.GetSection("Switches").Bind(setting.Switches);

            return null;
        }
    }
}
