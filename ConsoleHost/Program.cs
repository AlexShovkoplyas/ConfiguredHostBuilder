using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleHost
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = new HostBuilder()
              .ConfigureAppConfiguration((hostContext, config) =>
              {
                  config
                    .AddJsonFile("appConfig.json", false, true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);
              })
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddOptions();

                  //missing section
                  services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));

                  services.Configure<Person>(hostContext.Configuration.GetSection("Person"));

                  services.AddSingleton<IHostedService, PrintTextToConsoleService>();
              })
              .ConfigureLogging((hostContext, logging) => {
                  logging
                    .AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                    .AddConsole();
              });

            if (isService)
            {
                await builder.Build().RunAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }
    }
}
