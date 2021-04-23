using System.Collections.Generic;
using System.Linq;

namespace FaqDiscordBot.Options
{
    public class BotOptions
    {
        public class Providers
        {
            public const string Default = Lucene;

            public const string Lucene = nameof(Lucene);

            public const string QnaMaker = nameof(QnaMaker);

            public static IEnumerable<string> All()
            {
                yield return Lucene;
                yield return QnaMaker;
            }
        }

        public string Mode { get; set; } = Providers.Default;

        public string StatusMessage { get; set; } = "DM me your questions!";

        public double ConfidenceThreshold { get; set; } = 0.3;

        public bool SelfServiceEnabled { get; set; } = true;
    }
}
