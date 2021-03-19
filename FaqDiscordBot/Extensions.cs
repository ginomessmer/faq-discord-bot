using System.Collections.Generic;
using System.Linq;
using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Azure;

namespace FaqDiscordBot
{
    public static class Extensions
    {
        public static IAnswer GetBestAnswer(this IEnumerable<IAnswer> answers) => 
            answers.OrderByDescending(x => x.Score).FirstOrDefault();
    }
}