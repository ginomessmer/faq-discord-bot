using System.Collections.Generic;
using System.Linq;

namespace FaqDiscordBot.Models
{
    public class Question : Entity
    {
        public ICollection<Phrasing> Phrasings { get; set; } = new List<Phrasing>();

        public Answer Answer { get; set; }

        public ulong UserId { get; set; }

        public ulong MessageId { get; set; }

        public Question(string question, ulong userId, ulong messageId)
        {
            Phrasings = new List<Phrasing>
            {
                new Phrasing(question)
            };

            UserId = userId;
            MessageId = messageId;
        }

        public Question()
        {
        }

        public override string ToString() => Phrasings.First().Text;
    }
}
