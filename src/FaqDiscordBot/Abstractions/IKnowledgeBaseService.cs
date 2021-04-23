using FaqDiscordBot.Providers.Azure.Data;
using System.Threading.Tasks;

namespace FaqDiscordBot.Abstractions
{
    public interface IKnowledgeBaseService
    {
        Task ReplaceAsync(params QnaDto[] entries);
    }
}