using System.Collections.Generic;
using System.Threading.Tasks;

namespace QnaMakerDiscordBot.Abstractions
{
    public interface IFaqService
    {
        Task<IEnumerable<IAnswer>> AskAsync(string question);
    }
}