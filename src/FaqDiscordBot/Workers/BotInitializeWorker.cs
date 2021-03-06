using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using FaqDiscordBot.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FaqDiscordBot.Workers
{
    public class BotInitializeWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger<BotInitializeWorker> _logger;
        private readonly ILogger<DiscordSocketClient> _discordLogger;
        private readonly BotOptions _options;

        public BotInitializeWorker(DiscordSocketClient client, IOptions<BotOptions> options,
            ILogger<BotInitializeWorker> logger, ILogger<DiscordSocketClient> discordLogger)
        {
            _client = client;
            _logger = logger;
            _discordLogger = discordLogger;
            _options = options.Value;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ApplyCulture();
            ApplyLogger(stoppingToken);

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

        private void ApplyLogger(CancellationToken stoppingToken)
        {
            _client.Log += message => Task.Run(() => _discordLogger.Log(message.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Trace,
                LogSeverity.Warning => LogLevel.Warning,
                _ => LogLevel.Trace
            }, message.Exception, message.Message), stoppingToken);
        }
    }
}