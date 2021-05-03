using System;
using System.Globalization;

namespace FaqDiscordBot.Options
{
    public class BotOptions
    {
        public string StatusMessage { get; set; } = "DM me your questions!";

        public double ConfidenceThreshold { get; set; } = 0.3;

        public bool SelfServiceEnabled { get; set; } = true;

        public TimeSpan PurgeThreshold { get; set; } = TimeSpan.FromDays(3);

        public string CultureName { get; set; } = CultureInfo.CurrentCulture.Name;
    }
}
