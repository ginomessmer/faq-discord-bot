using FaqDiscordBot.Abstractions;

namespace FaqDiscordBot.Providers.Lucene
{
    public class LuceneAnswer : IAnswer
    {
        /// <inheritdoc />
        public string Answer { get; set; }

        /// <inheritdoc />
        public float Score { get; set; }
    }
}