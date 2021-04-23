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

namespace FaqDiscordBot.Providers
{
    public static class ProviderExtensions
    {
        public static IServiceCollection AddQnaMakerFaqProvider(this IServiceCollection services, IConfigurationSection configuration, string endpoint)
        {
            services.Configure<QnaMakerOptions>(configuration);
            services.AddHttpClient<IFaqService, QnaMakerFaqService>((sp, client) =>
                client.BaseAddress = new Uri(endpoint));

            return services;
        }
    }
}
