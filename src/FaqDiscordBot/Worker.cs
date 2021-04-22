using System;
using Discord;
using Discord.WebSocket;
using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            if (message.Channel is not IDMChannel)
                return;

            if (message.Author is ISelfUser || message.Author.IsBot || message.Author.IsWebhook)
                return;

            if (message is not IUserMessage userMessage)
                return;

            using var typing = message.Channel.EnterTypingState();
            var response = await _faqService.AskAsync(userMessage.Content);
            var answer = response.GetBestAnswer();

            if (answer is not null)
            {
                // Answer and end
                await userMessage.ReplyAsync(answer.Answer);
                return;
            }

            try
            {
                await mediator.Publish(new AnswerNotFoundEvent(userMessage));
                await SendFallbackReplyAsync(userMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async Task SendFallbackReplyAsync(IUserMessage message)
        {
            var replyMessage = await message.ReplyAsync(embed: new EmbedBuilder()
                .WithTitle("Ich konnte leider keine Antwort finden")
                .WithDescription("Aber du kannst deine Frage auf dem TINF Network stellen.\n" +
                                 "Antworte auf diese Nachricht sobald du eine passende Antwort hast, um sie in die Wissensdatenbank aufzunehmen.")
                .Build());
        }
    }
}
