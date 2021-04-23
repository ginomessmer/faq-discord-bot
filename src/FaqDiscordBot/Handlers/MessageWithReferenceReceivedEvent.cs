using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using FaqDiscordBot.Events;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Models;
using MediatR;

namespace FaqDiscordBot.Handlers
{
    public class AcknowledgeAddedAnswerEventHandler : INotificationHandler<AnswerAddedToQuestionEvent>
    {
        private readonly FaqDbContext _dbContext;
        private readonly IDiscordClient _client;

        public AcknowledgeAddedAnswerEventHandler(FaqDbContext dbContext, IDiscordClient client)
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
                .Build());
        }
    }

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