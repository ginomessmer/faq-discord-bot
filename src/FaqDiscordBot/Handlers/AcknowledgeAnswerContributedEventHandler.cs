using Discord;
using FaqDiscordBot.Events;
using FaqDiscordBot.Infrastructure;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FaqDiscordBot.Handlers
{
    public class AcknowledgeAnswerContributedEventHandler : INotificationHandler<AnswerAddedToQuestionEvent>
    {
        private readonly FaqDbContext _dbContext;
        private readonly IDiscordClient _client;

        public AcknowledgeAnswerContributedEventHandler(FaqDbContext dbContext, IDiscordClient client)
        {
            _dbContext = dbContext;
            _client = client;
        }

        public async Task Handle(AnswerAddedToQuestionEvent notification, CancellationToken cancellationToken)
        {
            var question = await _dbContext.Questions
                .FirstOrDefaultAsync(x => x.Id == notification.QuestionId, cancellationToken);

            if (question is null)
                return;

            var user = await _client.GetUserAsync(question.UserId);
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDescription($"Danke. Deine Antwort zu `{question}` wurde in die Wissensdatenbank aufgenommen.")
                .WithColor(Color.Green)
                .Build());
        }
    }
}