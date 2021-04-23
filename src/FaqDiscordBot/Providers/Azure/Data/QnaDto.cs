using System.Collections.Generic;

namespace FaqDiscordBot.Providers.Azure.Data
{
    public class QnaDto
    {
        /// <summary>
        /// Unique id for the Q-A.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Answer text
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Source from which Q-A was indexed. eg. https://docs.microsoft.com/en-us/azure/cognitive-services/QnAMaker/FAQs
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// List of questions associated with the answer.
        /// </summary>
        public IEnumerable<string> Questions { get; set; }

        /// <summary>
        /// List of metadata associated with the answer.
        /// </summary>
        public IEnumerable<MetadataDto> Metadata { get; set; }
    }
}
