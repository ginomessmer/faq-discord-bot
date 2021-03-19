using FaqDiscordBot.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace FaqDiscordBot
{
    public static class Extensions
    {
        public static IAnswer GetBestAnswer(this IEnumerable<IAnswer> answers) => 
            answers.OrderByDescending(x => x.Score).FirstOrDefault();
    }
}