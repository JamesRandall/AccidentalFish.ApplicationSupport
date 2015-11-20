using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Logging.Serilog;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;
using Serilog;
using ILogger = AccidentalFish.ApplicationSupport.Core.Logging.ILogger;

namespace SerilogSample
{
    class Program
    {
        static void Main(string[] args)
        {
            DefaultConfigurationSample();
            LogDirectlyWithSerilog();
            AccessSerilogWithCast();
            FactorySerilogConfiguration();
        }

        // The default configuration writes to the trace pipe.
        private static void DefaultConfigurationSample()
        {
            IUnityContainer container = new UnityContainer();
            IDependencyResolver dependencyResolver = new UnityApplicationFrameworkDependencyResolver(container);

            dependencyResolver
                .UseCore()
                .UseSerilog();

            ILoggerFactory loggerFactory = dependencyResolver.Resolve<ILoggerFactory>();
            ILogger sampleLogger = loggerFactory.CreateLogger();

            var structuredData = new { Hello = "World", SubObject = new { Some = "Bling" } };
            sampleLogger.Warning("A simple log item with data {@StructuredData}", structuredData);
        }

        // Demonstrates how to:
        //     Set the default minimum log level
        //     Log an item against a component
        //     How to get the underlying Serilog logger
        private static void LogDirectlyWithSerilog()
        {
            IUnityContainer container = new UnityContainer();
            IDependencyResolver dependencyResolver = new UnityApplicationFrameworkDependencyResolver(container);

            dependencyResolver
                .UseCore()
                .UseSerilog(defaultMinimumLogLevel:LogLevelEnum.Information);

            ISerilogFactory loggerFactory = dependencyResolver.Resolve<ISerilogFactory>();
            Serilog.ILogger sampleLogger = loggerFactory.CreateSerilog(new ComponentIdentity("accidentalfish.applicationsupport.samples.serilog"));

            var structuredData = new {Hello = "World", SubObject = new {Some = "Bling"}};
            sampleLogger.Information("A simple log item {@StructuredData} - {SourceFqn}", structuredData);
        }

        // Demonstrates how to access the underlying Serilog with a cast
        private static void AccessSerilogWithCast()
        {
            IUnityContainer container = new UnityContainer();
            IDependencyResolver dependencyResolver = new UnityApplicationFrameworkDependencyResolver(container);

            dependencyResolver
                .UseCore()
                .UseSerilog();

            ILoggerFactory loggerFactory = dependencyResolver.Resolve<ILoggerFactory>();
            Serilog.ILogger sampleLogger = (Serilog.ILogger)loggerFactory.CreateLogger();

            sampleLogger.Warning("Casts are handy");
        }

        // Demonstrates how to supply a custom basic log configuration to the factory
        private static void FactorySerilogConfiguration()
        {
            IUnityContainer container = new UnityContainer();
            IDependencyResolver dependencyResolver = new UnityApplicationFrameworkDependencyResolver(container);

            dependencyResolver
                .UseCore()
                .UseSerilog(
                    configurationProvider: () => new LoggerConfiguration().WriteTo.ColoredConsole(),
                    defaultMinimumLogLevel: LogLevelEnum.Information);

            ILoggerFactory loggerFactory = dependencyResolver.Resolve<ILoggerFactory>();
            ILogger sampleLogger = loggerFactory.CreateLogger(new ComponentIdentity("accidentalfish.applicationsupport.samples.serilog"));

            sampleLogger.Warning("Coloured console {SourceFqn}");
            sampleLogger.Error("Colourful {SourceFqn}");
        }
    }
}
