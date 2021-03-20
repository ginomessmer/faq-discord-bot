namespace FaqDiscordBot.Options
{
    public class BotOptions
    {
        public class Modes
        {
            public const string QnaMaker = "QnaMaker";

            public const string Local = "Local";
        }

        public string Mode { get; set; } = Modes.QnaMaker;

        public string StatusMessage { get; set; } = "DM me your questions!";
    }
}
