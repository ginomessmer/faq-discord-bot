using System;
using FaqDiscordBot.Events;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Models;
using FaqDiscordBot.Options;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace FaqDiscordBot.Handlers
{
    public class AddUnansweredQuestionToDatabaseEventHandler : INotificationHandler<AnswerNotFoundEvent>
    {
        private readonly FaqDbContext _dbContext;
        private readonly BotOptions _options;

        public AddUnansweredQuestionToDatabaseEventHandler(FaqDbContext dbContext, IOptions<BotOptions> options)
        {
            _dbContext = dbContext;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task Handle(AnswerNotFoundEvent notification, CancellationToken cancellationToken)
        {
            if (!_options.SelfServiceEnabled)
                return;


            var question = new Question(notification.QuestionMessage.Content,
                notification.QuestionMessage.Author.Id,
                notification.QuestionMessage.Id,
                _options.ReminderThreshold);

            await _dbContext.Questions.AddAsync(question, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}