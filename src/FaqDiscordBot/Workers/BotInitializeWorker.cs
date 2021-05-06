using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FaqDiscordBot.Workers
{
    public class BotInitializeWorker : BackgroundService
    {
        private readonly ILogger<BotInitializeWorker> _logger;
        private readonly BotOptions _options;

        public BotInitializeWorker(IOptions<BotOptions> options, ILogger<BotInitializeWorker> logger)
        {
            _logger = logger;
            _options = options.Value;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ApplyCulture();

            return Task.CompletedTask;
        }

        private void ApplyCulture()
        {
            try
            {
                var culture = CultureInfo.GetCultureInfo(_options.CultureName);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                _logger.LogInformation("Set culture to {CultureDisplayName}", culture.NativeName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to set culture to {CultureName}", _options.CultureName);
            }
        }
    }
}