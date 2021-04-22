namespace FaqDiscordBot.Abstractions
{
    public interface IAnswer
    {
        string Answer { get; }

        float ConfidenceScore { get; }
    }
}