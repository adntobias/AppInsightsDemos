using System;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CPUMeasure
{
    class Program
    {

        private static PerformanceCounter cpuCounter;
        private static string instrumetationKey = "84d334e8-7686-4252-9ed5-8f54e5d607b2";
        private static TelemetryClient telemetryClient;
        private static TelemetryConfiguration configuration;

        static async Task Main(string[] args)
        {
            configuration = TelemetryConfiguration.CreateDefault();
            configuration.InstrumentationKey = instrumetationKey;

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            //var telemetryClient = new TelemetryClient(configuration);
           
            // Create the DI container.
            IServiceCollection services = new ServiceCollection();

            // Being a regular console app, there is no appsettings.json or configuration providers enabled by default.
            // Hence instrumentation key and any changes to default logging level must be specified here.
            services.AddLogging(loggingBuilder => loggingBuilder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>("Category", LogLevel.Information));
            services.AddApplicationInsightsTelemetryWorkerService(instrumetationKey);

            // Build ServiceProvider.
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Obtain logger instance from DI.
            ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            // Obtain TelemetryClient instance from DI, for additional manual tracking or to flush.
            var telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();
            
            while(true)
            {
                var value = cpuCounter.NextValue();
                System.Console.WriteLine($"CPU Value: {value}");
                telemetryClient.TrackMetric("cpuUsage", value);

                if(value > 80.0)
                {
                    telemetryClient.TrackEvent("highCPUUsage");
                }
                
                await Task.Delay(1000);
            }

            telemetryClient.Flush();
            Task.Delay(5000).Wait();
        }
    }
}
