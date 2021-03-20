using System.Collections.Generic;

namespace FaqDiscordBot.Providers.Azure
{
    public class QnaMakerQuestionResponse
    {
        public IEnumerable<QnaMakerAnswerResponse> Answers { get; set; }
        public bool ActiveLearningEnabled { get; set; }
    }
}