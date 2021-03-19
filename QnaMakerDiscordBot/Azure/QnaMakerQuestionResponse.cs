using System.Collections.Generic;
using System.Linq;
using QnaMakerDiscordBot.Azure;

public class QnaMakerQuestionResponse
{
    public IEnumerable<QnaMakerAnswerResponse> Answers { get; set; }
    public bool ActiveLearningEnabled { get; set; }

    public QnaMakerAnswerResponse GetBestAnswer() => Answers.OrderByDescending(x => x.Score).FirstOrDefault();
}