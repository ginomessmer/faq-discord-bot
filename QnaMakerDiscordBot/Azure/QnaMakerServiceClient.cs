using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using QnaMakerDiscordBot.Abstractions;

namespace QnaMakerDiscordBot.Azure
{
    public class QnaMakerServiceClient : IFaqService
    {
        private readonly HttpClient _httpClient;
        private readonly QnaMakerOptions _options;

        public QnaMakerServiceClient(HttpClient httpClient, IOptions<QnaMakerOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _options.SubscriptionKey);
        }

        public async Task<IEnumerable<IAnswer>> AskAsync(string question)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"/qnamaker/v5.0-preview.1/knowledgebases/{_options.KnowledgeBaseId}/generateAnswer", new
                {
                    question
                });

            response.EnsureSuccessStatusCode();
            var answer = await response.Content.ReadFromJsonAsync<QnaMakerQuestionResponse>();
            return answer?.Answers;
        }
    }
}