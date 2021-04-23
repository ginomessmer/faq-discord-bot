using MediatR;

namespace FaqDiscordBot.Events
{
    public record AnswerAddedToQuestionEvent(int QuestionId) : INotification;
}