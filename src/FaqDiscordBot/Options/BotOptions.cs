﻿using System.Collections.Generic;

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
                yield return Default;
                yield return Lucene;
                yield return QnaMaker;
            }
        }

        public string Mode { get; set; } = Providers.Default;

        public string StatusMessage { get; set; } = "DM me your questions!";
    }
}