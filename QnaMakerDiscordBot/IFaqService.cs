using System.Threading.Tasks;

namespace QnaMakerDiscordBot
{
    public interface IFaqService
    {
        Task<QnaMakerQuestionResponse> AskAsync(string question);
    }
}