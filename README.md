# IsolatedStorageLogger
Isolated storage is a data storage machanism for Universal Windows Platform (UWP). More read about isolated storage click [here](https://docs.microsoft.com/en-us/dotnet/standard/io/isolated-storage). 
IsolatedStorageLogger is a implementation of .NET Core ILogger for UWP isolated storage.

# NuGet Package
To install Daenet.Common.Logging.IsolatedStorageLogger, run the following command in the Package Manager Console 
````
Install-Package Daenet.Common.Logging.IsolatedStorageLogger
````

To read more about Daenet.Common.Logging.IsolatedStorageLogger go to [NuGet Package](https://www.nuget.org/packages/Daenet.Common.Logging.IsolatedStorageLogger).

# Configuration 
Before using IsolatedStorageLogger you have to configure bellow showing how to configure

````C#
private void init(Func<string, LogLevel, bool> filter,
      Func<LogLevel, EventId, object, Exception, string> eventDataFormatter = null,
   Dictionary<string, object> additionalValues = null)
{
    Dictionary<string, LogLevel> dictionary = new Dictionary<string, LogLevel>();
    dictionary.Add("UnitTests", 0);

    var setting = new IsolatedStorageLoggerSettings()
    {
        // Directory name in Isolated storage where log file will be created.
        Directory = "IsolatedStrorageLog",

        //File name where log information will be saved
        FileName = "log.json",

        Switches = dictionary
    };

    ILoggerFactory loggerFactory = new LoggerFactory()
        .AddIsolatedStorage(setting, filter, eventDataFormatter, additionalValues);

    //Creates a new ILogger instance
    m_Logger = loggerFactory.CreateLogger<UnitTest1>();
}
````

# Log Output
Log out put will look like this 
````json
{
	"Name": "UnitTests.UnitTest1",
	"Scope": null,
	"EventId": "0",
	"Message": "Test Trace Log Message",
	"Level": 0,
	"LocalEnqueuedTime": "2017-07-24T12:16:39.6724484+02:00",
	"Exception": null
}
````