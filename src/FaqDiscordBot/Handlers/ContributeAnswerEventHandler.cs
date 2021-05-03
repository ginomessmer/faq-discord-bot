using FaqDiscordBot.Events;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace FaqDiscordBot.Handlers
{
    public class ContributeAnswerEventHandler : INotificationHandler<MessageWithReferenceReceivedEvent>
    {
        private readonly FaqDbContext _dbContext;
        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger<ContributeAnswerEventHandler> _logger;
        private readonly IMediator _mediator;

        public ContributeAnswerEventHandler(FaqDbContext dbContext, TelemetryClient telemetryClient,
            ILogger<ContributeAnswerEventHandler> logger,
            IMediator mediator)
        {
            _dbContext = dbContext;
            _telemetryClient = telemetryClient;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Handle(MessageWithReferenceReceivedEvent notification, CancellationToken cancellationToken)
        {
            var (userMessage, referencedMessage) = notification;

            var question = await _dbContext.Questions.
                FirstOrDefaultAsync(x => x.MessageId == referencedMessage.Id,
                    cancellationToken);

            if (question is null)
                return;

            question.Answer = new Answer(userMessage.Content, userMessage.Id, userMessage.Author.Id);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Answer contributed to question {QuestionId} by {UserId}",
                question.Id, userMessage.Author.Id);
            _telemetryClient.TrackEvent("Answer contributed");

            await _mediator.Publish(new AnswerAddedToQuestionEvent(question.Id), cancellationToken);
        }
    }
}