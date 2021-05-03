using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Models;
using FaqDiscordBot.Properties;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FaqDiscordBot.Workers
{
    public class InitializeDatabaseWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KnowledgeBaseUpdateWorker> _logger;

        public InitializeDatabaseWorker(IServiceProvider serviceProvider,
            ILogger<KnowledgeBaseUpdateWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            await using var dbContext = scope.ServiceProvider.GetRequiredService<FaqDbContext>();

            if (await dbContext.Questions.AnyAsync(stoppingToken))
            {
                _logger.LogInformation("DB already contains entries. Skipping seeding...");
                return;
            }


            // Add tutorial message
            await dbContext.Questions.AddAsync(new Question("tutorial", 0, 0)
            {
                Answer = new Answer(Resources.UnansweredQuestionInstructionsText_Desc_Rookie, 0, 0)
            }, stoppingToken);

            // Pull from KB
            var knowledgeBaseService = scope.ServiceProvider.GetRequiredService<IKnowledgeBaseService>();

            var entries = await knowledgeBaseService.DownloadAsync();
            var items = entries.Select(x => new Question
            {
                Answer = new Answer { Text = x.Answer },
                Phrasings = x.Questions.Select(q => new Phrasing(q)).ToList(),
                IsApproved = true
            }).ToList();

            await dbContext.Questions.AddRangeAsync(items, stoppingToken);

            await dbContext.SaveChangesAsync(stoppingToken);
            _logger.LogInformation("Seeded the database with {EntriesCount} using the existing remote KB", items.Count);
        }
    }
}