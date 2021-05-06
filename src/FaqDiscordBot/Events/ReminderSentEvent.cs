using System.Collections.Generic;
using MediatR;

namespace FaqDiscordBot.Events
{
    public record ReminderSentEvent(IEnumerable<int> QuestionIds) : INotification;
}