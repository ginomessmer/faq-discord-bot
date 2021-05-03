using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FaqDiscordBot.Workers
{
    public class BotInitializeWorker : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }

    public class AutoPurgeWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AutoPurgeWorker> _logger;
        private readonly BotOptions _options;

        public AutoPurgeWorker(IServiceProvider serviceProvider, IOptions<BotOptions> options,
            ILogger<AutoPurgeWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                await using var dbContext = scope.ServiceProvider.GetRequiredService<FaqDbContext>();

                var danglingQuestions = await dbContext.Questions.AsQueryable()
                    .Where(x => x.Answer == null && x.CreatedAt > DateTime.UtcNow + _options.PurgeThreshold)
                    .ToListAsync(stoppingToken);

                dbContext.Questions.RemoveRange(danglingQuestions);
                await dbContext.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("Purged {DanglingQuestionsCount} dangling questions", danglingQuestions.Count);
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}