using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Events;
using FaqDiscordBot.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaqDiscordBot.Handlers
{
    public class RemoveRemindersEventHandler : INotificationHandler<ReminderSentEvent>
    {
        private readonly FaqDbContext _dbContext;

        public RemoveRemindersEventHandler(FaqDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(ReminderSentEvent notification, CancellationToken cancellationToken)
        {
            if (!notification.QuestionIds.Any())
                return;

            var questions = await _dbContext.Questions.AsQueryable()
                .Where(x => notification.QuestionIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            questions.ForEach(x => x.Meta.ReminderAt = null);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}