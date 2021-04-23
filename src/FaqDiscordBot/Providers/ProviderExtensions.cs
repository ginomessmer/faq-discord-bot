using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Options;
using FaqDiscordBot.Providers.Azure;
using Lucene.Net.Analysis.De;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;

namespace FaqDiscordBot.Providers
{
    public static class ProviderExtensions
    {
        public static IServiceCollection AddQnaMakerFaqProvider(this IServiceCollection services, IConfigurationSection configuration, string endpoint)
        {
            services.Configure<QnaMakerOptions>(configuration);

            void ConfigureClient(IServiceProvider sp, HttpClient client) => client.BaseAddress = new Uri(endpoint);
            services.AddHttpClient<IFaqService, QnaMakerFaqService>(ConfigureClient);
            services.AddHttpClient<IKnowledgeBaseService, QnaMakerKnowledgeBaseService>(ConfigureClient);

            return services;
        }
    }
}
