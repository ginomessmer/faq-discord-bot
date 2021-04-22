using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Events;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Models;
using MediatR;

namespace FaqDiscordBot.Handlers
{
    public class AddAnswerToQuestionEventHandler : INotificationHandler<MessageWithReferenceReceivedEvent>
    {
        private readonly FaqDbContext _dbContext;

        public AddAnswerToQuestionEventHandler(FaqDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(MessageWithReferenceReceivedEvent notification, CancellationToken cancellationToken)
        {
            var (userMessage, referencedMessage) = notification;

            var question = await _dbContext.Questions.
                FirstOrDefaultAsync(x => x.MessageId == referencedMessage.Id,
                    cancellationToken);

            if (question is null)
                return;

            question.Answer = new Answer(userMessage.Content);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}