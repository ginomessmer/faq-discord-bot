using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using FaqDiscordBot.Events;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Models;
using FaqDiscordBot.Options;
using FaqDiscordBot.Properties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FaqDiscordBot.Workers
{
    public class DanglingQuestionsReminderWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly ILogger<DanglingQuestionsAutoPurgeWorker> _logger;
        private readonly BotOptions _options;

        public DanglingQuestionsReminderWorker(IServiceProvider serviceProvider,
            IMediator mediator,
            IOptions<BotOptions> options,
            ILogger<DanglingQuestionsAutoPurgeWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    await using var dbContext = scope.ServiceProvider.GetRequiredService<FaqDbContext>();
                    using var discordClient = scope.ServiceProvider.GetRequiredService<IDiscordClient>();

                    var questions = await GetDanglingQuestionsAsync(dbContext, stoppingToken);

                    foreach (var question in questions)
                    {
                        try
                        {
                            var user = await discordClient.GetUserAsync(question.UserId);
                            if (user is null)
                                continue;

                            await SendReminderDirectMessageAsync(user, question);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Couldn't send reminder DM to user {UserId}", question.UserId);
                        }
                    }

                    await _mediator.Publish(new ReminderSentEvent(questions.Select(x => x.Id)), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Couldn't send pending reminders. Trying again later...");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task<List<Question>> GetDanglingQuestionsAsync(FaqDbContext dbContext, CancellationToken stoppingToken)
        {
            return await dbContext.Questions.AsQueryable()
                .Where(x => x.Meta.ReminderAt <= DateTime.UtcNow
                            && x.Answer == null
                            && x.UserId != 0
                            && x.CreatedAt + _options.PurgeThreshold >= DateTime.UtcNow)
                .ToListAsync(stoppingToken);
        }

        private static async Task SendReminderDirectMessageAsync(IUser user, Question question)
        {
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            var message = await dmChannel.GetMessageAsync(question.Meta.MessageId);

            await dmChannel.SendMessageAsync(embed: new EmbedBuilder()
                .WithTitle(Resources.UnansweredQuestionReminderEmbed_Title)
                .WithDescription(
                    new StringBuilder()
                        .AppendLine($"> {question}")
                        .AppendLine($"> _{message.GetJumpUrl()}_")
                        .AppendLine()
                        .AppendLine(Resources.UnansweredQuestionReminderEmbed_Desc)
                        .ToString())
                .WithColor(Color.Orange)
                .Build());
        }
    }
}