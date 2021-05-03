using System;

namespace FaqDiscordBot.Models
{
    public class QuestionMeta
    {
        public ulong MessageId { get; set; }

        public DateTime LastReminderAt { get; set; }
    }
}