using System;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FaqDiscordBot.Workers
{
    public class DanglingQuestionsReminderWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DanglingQuestionsAutoPurgeWorker> _logger;
        private readonly BotOptions _options;

        public DanglingQuestionsReminderWorker(IServiceProvider serviceProvider,
            IMemoryCache cache,
            IOptions<BotOptions> options,
            ILogger<DanglingQuestionsAutoPurgeWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
            _options = options.Value;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
            }
            return Task.CompletedTask;
        }
    }
}