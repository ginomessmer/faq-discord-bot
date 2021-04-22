namespace FaqDiscordBot.Models
{
    public class Answer
    {
        public Answer(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}