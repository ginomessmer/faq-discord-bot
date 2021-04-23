using Discord;
using Discord.WebSocket;
using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Events;
using FaqDiscordBot.Options;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FaqDiscordBot
{
    public class Worker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IFaqService _faqService;
        private readonly IServiceProvider _serviceProvider;
        private readonly BotOptions _botOptions;
        private readonly ILogger<Worker> _logger;

        public Worker(DiscordSocketClient client, IFaqService faqService,
            IOptions<BotOptions> botOptions,
            IServiceProvider serviceProvider,
            ILogger<Worker> logger)
        {
            _client = client;
            _faqService = faqService;
            _serviceProvider = serviceProvider;
            _botOptions = botOptions.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.MessageReceived += ClientOnMessageReceived;
            await _client.SetGameAsync(_botOptions.StatusMessage);
        }

        private async Task ClientOnMessageReceived(SocketMessage message)
        {
            // Check
            if (message.Channel is not IDMChannel)
                return;

            if (message.Author is ISelfUser || message.Author.IsBot || message.Author.IsWebhook)
                return;

            if (message is not IUserMessage userMessage)
                return;

            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(new DmReceivedEvent(userMessage));
        }
    }
}
