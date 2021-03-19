using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using QnaMakerDiscordBot.Abstractions;
using QnaMakerDiscordBot.Options;
using MessageActivity = Discord.API.MessageActivity;

namespace QnaMakerDiscordBot
{
    public class Worker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IFaqService _faqService;
        private readonly BotOptions _botOptions;
        private readonly ILogger<Worker> _logger;

        public Worker(DiscordSocketClient client, IFaqService faqService,
            IOptions<BotOptions> botOptions,
            ILogger<Worker> logger)
        {
            _client = client;
            _faqService = faqService;
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
            if (message.Channel is not IDMChannel)
                return;

            if (message.Author is ISelfUser || message.Author.IsBot || message.Author.IsWebhook)
                return;

            using var typing = message.Channel.EnterTypingState();
            var response = await _faqService.AskAsync(message.Content);
            var answer = response.GetBestAnswer();

            await message.Channel.SendMessageAsync(answer.ToString());
        }
    }
}
