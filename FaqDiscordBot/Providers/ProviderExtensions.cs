﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Options;
using FaqDiscordBot.Providers.Azure;
using FaqDiscordBot.Providers.Local;
using Lucene.Net.Analysis.De;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FaqDiscordBot.Providers
{
    public static class ProviderExtensions
    {
        public static IServiceCollection AddFaqProvider(this IServiceCollection services, IConfiguration configuration,
            string provider = BotOptions.Modes.Default)
        {
            return provider switch
            {
                BotOptions.Modes.QnaMaker => services.AddQnaMakerFaqProvider(configuration),
                BotOptions.Modes.Lucene => services.AddLuceneProvider(),
                _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, 
                    $"Unrecognized provider. Use one of {string.Join(", ", BotOptions.Modes.All())}")
            };
        }

        public static IServiceCollection AddQnaMakerFaqProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<QnaMakerOptions>(configuration.GetSection("QnaMaker"));
            services.AddHttpClient<IFaqService, QnaMakerFaqService>((sp, client) =>
                client.BaseAddress = new Uri(configuration.GetConnectionString("QnaServiceEndpoint")));

            return services;
        }

        public static IServiceCollection AddLuceneProvider(this IServiceCollection services)
        {
            services.AddSingleton<GermanAnalyzer>(_ => new GermanAnalyzer(LuceneVersion.LUCENE_48));

            services.AddSingleton<IndexWriterConfig>(sp =>
                new IndexWriterConfig(LuceneVersion.LUCENE_48, sp.GetRequiredService<GermanAnalyzer>()));

            services.AddSingleton<IndexWriter>(sp => new IndexWriter(
                FSDirectory.Open(Path.Combine(Environment.CurrentDirectory, "data", "index")),
                sp.GetRequiredService<IndexWriterConfig>()));

            services.AddTransient<IndexSearcher>(sp =>
                new IndexSearcher(sp.GetRequiredService<IndexWriter>().GetReader(true)));

            services.AddSingleton<IFaqService, LuceneFaqService>();

            services.AddHostedService<LuceneIndexerWorker>();

            return services;
        }
    }
}
