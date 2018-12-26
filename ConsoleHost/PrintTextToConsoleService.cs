using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsoleHost
{
    public class PrintTextToConsoleService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<Person> _personFromConfig;
        private Timer _timer;

        public PrintTextToConsoleService(ILogger<PrintTextToConsoleService> logger, IOptions<Person> personFromConfig)
        {
            _logger = logger;
            _personFromConfig = personFromConfig;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation($"Background work with text: {_personFromConfig.Value.FullName}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}