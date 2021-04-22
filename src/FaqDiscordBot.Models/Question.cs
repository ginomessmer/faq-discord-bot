using System.Collections.Generic;

namespace FaqDiscordBot.Models
{
    public class Question : Entity
    {
        public Question(string question, ulong askedBy)
        {
            Phrasings = new List<Phrasing>
            {
                new Phrasing(question)
            };

            AskedBy = askedBy;
        }

        public Question()
        {
        }

        public ICollection<Phrasing> Phrasings { get; set; } = new List<Phrasing>();

        public Answer Answer { get; set; }

        public ulong AskedBy { get; set; }
    }
}
