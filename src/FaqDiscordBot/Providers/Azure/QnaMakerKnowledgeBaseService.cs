using System.Collections.Generic;
using System.Linq;
using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Providers.Azure.Data;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FaqDiscordBot.Models;

namespace FaqDiscordBot.Providers.Azure
{
    public class QnaMakerKnowledgeBaseService : IKnowledgeBaseService
    {
        private readonly HttpClient _httpClient;
        private readonly QnaMakerOptions _options;

        public QnaMakerKnowledgeBaseService(HttpClient httpClient, IOptions<QnaMakerOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _options.SubscriptionKey);
        }

        public async Task ReplaceAsync(IEnumerable<QnaDto> entries)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"/qnamaker/v5.0-preview.1/knowledgebases/{_options.KnowledgeBaseId}", 
                new
                {
                    qnAList = entries
                });

            var res = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<QnaDto>> DownloadAsync(bool pullProduction = true)
        {
            var environment = pullProduction ? "Prod" : "Test";
            var response = await _httpClient.GetFromJsonAsync<DownloadKbDto>($"/qnamaker/v5.0-preview.1/knowledgebases/{_options.KnowledgeBaseId}/{environment}/qna");

            return response?.QnaDocuments;
        }
    }
}