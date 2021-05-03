using Discord;
using MediatR;

namespace FaqDiscordBot.Events
{
    public record DmReceivedEvent(IUserMessage Message) : INotification;
}