using FaqDiscordBot.Events;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FaqDiscordBot.Handlers
{
    public class AddAnswerToQuestionEventHandler : INotificationHandler<MessageWithReferenceReceivedEvent>
    {
        private readonly FaqDbContext _dbContext;
        private readonly IMediator _mediator;

        public AddAnswerToQuestionEventHandler(FaqDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
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

            await _mediator.Publish(new AnswerAddedToQuestionEvent(question.Id), cancellationToken);
        }
    }
}