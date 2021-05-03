using FaqDiscordBot.Abstractions;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FaqDiscordBot.Providers.Azure
{
    public class QnaMakerAnswerResponse : IAnswer
    {
        public IEnumerable<object> Questions { get; set; }
        public string Answer { get; set; }
        public float Score { get; set; }
        public int Id { get; set; }
        public bool IsDocumentText { get; set; }
        public IEnumerable<object> Metadata { get; set; }

        [JsonIgnore]
        public float ConfidenceScore => Score / 100;

        public override string ToString() => Answer;
    }
}