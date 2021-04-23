namespace FaqDiscordBot.Options
{
    public class BotOptions
    {
        public string StatusMessage { get; set; } = "DM me your questions!";

        public double ConfidenceThreshold { get; set; } = 0.3;

        public bool SelfServiceEnabled { get; set; } = true;
    }
}
