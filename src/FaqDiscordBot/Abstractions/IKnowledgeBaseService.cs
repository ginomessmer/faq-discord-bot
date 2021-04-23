using System.Collections.Generic;
using System.Threading.Tasks;
using FaqDiscordBot.Providers.Azure.Data;

namespace FaqDiscordBot.Abstractions
{
    public interface IKnowledgeBaseService
    {
        Task ReplaceAsync(IEnumerable<QnaDto> entries);
    }
}