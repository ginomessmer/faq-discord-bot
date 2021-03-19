using System.Collections.Generic;

namespace QnaMakerDiscordBot.Azure
{
    public class QnaMakerQuestionResponse
    {
        public IEnumerable<QnaMakerAnswerResponse> Answers { get; set; }
        public bool ActiveLearningEnabled { get; set; }
    }
}