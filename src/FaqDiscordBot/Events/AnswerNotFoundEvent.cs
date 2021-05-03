using Discord;
using MediatR;

namespace FaqDiscordBot.Events
{
    public record AnswerNotFoundEvent(IUserMessage QuestionMessage) : INotification;
}