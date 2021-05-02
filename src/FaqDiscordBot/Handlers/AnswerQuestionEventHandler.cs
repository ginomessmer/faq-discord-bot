using System.Threading;
using System.Threading.Tasks;
using Discord;
using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Events;
using FaqDiscordBot.Extensions;
using FaqDiscordBot.Options;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Options;

namespace FaqDiscordBot.Handlers
{
    public class AnswerQuestionEventHandler : INotificationHandler<DmReceivedEvent>
    {
        private readonly IMediator _mediator;
        private readonly IFaqService _faqService;
        private readonly TelemetryClient _telemetryClient;
        private readonly BotOptions _options;

        public AnswerQuestionEventHandler(IMediator mediator, IFaqService faqService,
            TelemetryClient telemetryClient,
            IOptions<BotOptions> options)
        {
            _mediator = mediator;
            _faqService = faqService;
            _telemetryClient = telemetryClient;
            _options = options.Value;
        }

        public async Task Handle(DmReceivedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Message.Reference is not null && notification.Message.Reference.MessageId.IsSpecified)
            {
                // Check if it's related to a question
                await _mediator.Publish(new MessageWithReferenceReceivedEvent(
                        notification.Message, 
                        notification.Message.ReferencedMessage),
                    cancellationToken);

                return;
            }

            using var typing = notification.Message.Channel.EnterTypingState();
            using var holder = _telemetryClient.StartOperation<RequestTelemetry>("QnA Maker Request");

            // Ask
            var response = await _faqService.AskAsync(notification.Message.Content);
            var answer = response.GetBestAnswer();

            if (answer is not null && answer.ConfidenceScore >= _options.ConfidenceThreshold)
            {
                _telemetryClient.TrackEvent("Answer found");

                // Answer and end
                await notification.Message.ReplyAsync(answer.Answer);
                return;
            }


            _telemetryClient.TrackEvent("Answer not found");
            
            await _mediator.Publish(new AnswerNotFoundEvent(notification.Message), cancellationToken);
            await SendFallbackReplyAsync(notification.Message);
        }

        private async Task SendFallbackReplyAsync(IUserMessage message)
        {
            var replyMessage = await message.ReplyAsync(embed: new EmbedBuilder()
                .WithTitle("Ich konnte leider keine Antwort finden")
                .WithDescription("Aber du kannst deine Frage auf dem TINF Network stellen.\n" +
                                 "Antworte auf deine eigene Nachricht oben sobald du eine passende Antwort hast, um sie in die Wissensdatenbank aufzunehmen.")
                .Build());
        }
    }
}