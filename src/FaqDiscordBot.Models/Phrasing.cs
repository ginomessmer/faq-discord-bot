namespace FaqDiscordBot.Models
{
    public class Phrasing
    {
        public string Text { get; set; }

        public Phrasing(string text)
        {
            Text = text;
        }

        public Phrasing()
        {
        }
    }
}