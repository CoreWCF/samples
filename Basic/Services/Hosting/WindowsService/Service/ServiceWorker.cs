// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.WindowsService
{
    public class WindowsServiceWorker : BackgroundService
    {
        private readonly ILogger<WindowsServiceWorker> _logger;

        public WindowsServiceWorker(ILogger<WindowsServiceWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service is starting.");
            stoppingToken.Register(() => _logger.LogInformation("Service is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("CoreWCF Service is running");
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Service has stopped.");
        }
    }
}
