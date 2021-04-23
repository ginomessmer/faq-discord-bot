namespace FaqDiscordBot.Models
{
    public class Answer
    {
        public Answer(string text, ulong messageId, ulong userId)
        {
            Text = text;
            MessageId = messageId;
            UserId = userId;
        }

        public string Text { get; set; }

        public ulong MessageId { get; set; }

        public ulong UserId { get; set; }
    }
}