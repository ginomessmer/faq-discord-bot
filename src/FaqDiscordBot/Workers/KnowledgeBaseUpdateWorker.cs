using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Providers.Azure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FaqDiscordBot.Workers
{
    public class KnowledgeBaseUpdateWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KnowledgeBaseUpdateWorker> _logger;

        public KnowledgeBaseUpdateWorker(IServiceProvider serviceProvider, ILogger<KnowledgeBaseUpdateWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                await using var dbContext = scope.ServiceProvider.GetRequiredService<FaqDbContext>();
                var knowledgeBaseService = scope.ServiceProvider.GetRequiredService<IKnowledgeBaseService>();

                var entries = await dbContext.Questions.AsQueryable()
                    .Where(x => x.IsApproved)
                    .Select(x => new QnaDto
                    {
                        Answer = x.Answer.Text,
                        Questions = x.Phrasings.Select(x => x.Text),
                        Source = "Discord",
                        Metadata = new List<MetadataDto>
                        {
                            new(nameof(x.CreatedAt), x.CreatedAt.ToFileTimeUtc().ToString()),
                            new(nameof(x.UserId), x.UserId.ToString())
                        }
                    })
                    .ToListAsync(stoppingToken);

                if (entries.Any())
                {
                    try
                    {
                        await knowledgeBaseService.ReplaceAsync(entries);
                        _logger.LogInformation("Updated {EntriesCount} entries in knowledge base", entries.Count);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Couldn't update knowledge base");
                    }
                }
                else
                {
                    _logger.LogInformation("No approved entries found to publish to knowledge base", entries.Count);
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}