using Discord;
using MediatR;

namespace FaqDiscordBot.Events
{
    public record MessageWithReferenceReceivedEvent
        (IUserMessage Message, IUserMessage ReferencedMessage) : INotification;
}