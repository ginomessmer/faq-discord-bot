using System;
using System.Collections.Generic;
using System.Linq;

namespace FaqDiscordBot.Models
{
    public class Question : Entity
    {
        public ICollection<Phrasing> Phrasings { get; set; } = new List<Phrasing>();

        public Answer Answer { get; set; }

        public ulong UserId { get; set; }

        public QuestionMeta Meta { get; set; } = new();

        public bool IsApproved { get; set; }

        public Question(string question, ulong userId, ulong messageId)
        {
            Phrasings = new List<Phrasing>
            {
                new Phrasing(question)
            };

            UserId = userId;

            Meta = new QuestionMeta
            {
                MessageId = messageId
            };
        }

        public Question(string question, ulong userId, ulong messageId, TimeSpan reminderIn) : this(question, userId, messageId)
        {
            Meta.ReminderAt = DateTime.UtcNow.Add(reminderIn);
        }

        public Question()
        {
        }

        public override string ToString() => Phrasings.First().Text;
    }
}
