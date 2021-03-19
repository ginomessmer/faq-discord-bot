using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace QnaMakerDiscordBot.Azure
{
    public class QnaMakerServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly QnaMakerOptions _options;

        public QnaMakerServiceClient(HttpClient httpClient, IOptions<QnaMakerOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _options.SubscriptionKey);
            _httpClient.DefaultRequestHeaders.Add("Content-Type", MediaTypeNames.Application.Json);
        }

        public async Task<QnaMakerQuestionResponse> AskAsync(string question)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"/knowledgebases/{_options.KnowledgeBaseId}/generateAnswer", new
                {
                    question
                });

            return await response.Content.ReadFromJsonAsync<QnaMakerQuestionResponse>();
        }
    }
}