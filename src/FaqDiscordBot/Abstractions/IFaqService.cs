using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaqDiscordBot.Abstractions
{
    public interface IFaqService
    {
        Task<IEnumerable<IAnswer>> AskAsync(string question);
    }
}