using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Events;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Models;
using MediatR;

namespace FaqDiscordBot.Handlers
{
    public class AddUnansweredQuestionToDatabaseEventHandler : INotificationHandler<AnswerNotFoundEvent>
    {
        private readonly FaqDbContext _dbContext;

        public AddUnansweredQuestionToDatabaseEventHandler(FaqDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task Handle(AnswerNotFoundEvent notification, CancellationToken cancellationToken)
        {
            await _dbContext.Questions.AddAsync(new Question(notification.QuestionMessage.Content, notification.QuestionMessage.Author.Id), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}