using System.Collections.Generic;
using System.Linq;
using FaqDiscordBot.Abstractions;

namespace FaqDiscordBot.Extensions
{
    public static class AnswerExtensions
    {
        public static IAnswer GetBestAnswer(this IEnumerable<IAnswer> answers) => answers
            .OrderByDescending(x => x.ConfidenceScore)
            .FirstOrDefault(x => x.ConfidenceScore > 0);
    }
}