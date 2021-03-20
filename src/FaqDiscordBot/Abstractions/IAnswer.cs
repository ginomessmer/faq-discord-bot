namespace FaqDiscordBot.Abstractions
{
    public interface IAnswer
    {
        string Answer { get; set; }

        float Score { get; set; }
    }
}