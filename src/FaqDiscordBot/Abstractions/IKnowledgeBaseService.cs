using System.Threading.Tasks;
using FaqDiscordBot.Providers.Azure.Data;

namespace FaqDiscordBot.Abstractions
{
    public interface IKnowledgeBaseService
    {
        Task ReplaceAsync(params QnaDto[] entries);
    }
}