using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Providers.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
