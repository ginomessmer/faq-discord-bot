using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using FaqDiscordBot.Events;
using FaqDiscordBot.Properties;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace FaqDiscordBot.Handlers
{
    public class ReplyInstructionsOnUnansweredQuestionEventHandler : INotificationHandler<AnswerNotFoundEvent>
    {
        private readonly IMemoryCache _cache;

        public ReplyInstructionsOnUnansweredQuestionEventHandler(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <inheritdoc />
        public async Task Handle(AnswerNotFoundEvent notification, CancellationToken cancellationToken)
        {
            var key = "quickstart_" + notification.QuestionMessage.Author.Id;
            
            if (!_cache.TryGetValue(key, out var val))
            {
                await notification.QuestionMessage.ReplyAsync(embed: new EmbedBuilder()
                    .WithTitle(Resources.UnansweredQuestionInstructionsText_Title_Rookie)
                    .WithDescription(Resources.UnansweredQuestionInstructionsText_Desc_Rookie)
                    .WithColor(Color.Red)
                    .WithImageUrl(Resources.UnansweredQuestionInstructionsImage_Url)
                    .Build());

                _cache.Set(key, notification.QuestionMessage.Id, TimeSpan.FromDays(3));
                return;
            }

            await notification.QuestionMessage.ReplyAsync(embed: new EmbedBuilder()
                .WithTitle(Resources.UnansweredQuestionInstructionsText_Title_Pro)
                .WithDescription(Resources.UnansweredQuestionInstructionsText_Desc_Pro)
                .WithColor(Color.Red)
                .Build());
        }
    }
}