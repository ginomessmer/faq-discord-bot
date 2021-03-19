using System.Collections.Generic;
using System.Linq;
using QnaMakerDiscordBot.Abstractions;
using QnaMakerDiscordBot.Azure;

namespace QnaMakerDiscordBot
{
    public static class Extensions
    {
        public static IAnswer GetBestAnswer(this IEnumerable<IAnswer> answers) => 
            answers.OrderByDescending(x => x.Score).FirstOrDefault();
    }
}