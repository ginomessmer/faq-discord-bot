using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace QnaMakerDiscordBot
{
    public class Worker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IFaqService _faqService;
        private readonly ILogger<Worker> _logger;

        public Worker(DiscordSocketClient client, IFaqService faqService, ILogger<Worker> logger)
        {
            _client = client;
            _faqService = faqService;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.MessageReceived += ClientOnMessageReceived;
            return Task.CompletedTask;
        }

        private async Task ClientOnMessageReceived(SocketMessage message)
        {
            var response = await _faqService.AskAsync(message.Content);
            var answer = response.GetBestAnswer();

            if (message is not IUserMessage userMessage || message.Author.IsBot || message.Author.IsWebhook)
                return;

            await userMessage.Channel.SendMessageAsync(answer.ToString());
        }
    }
}
