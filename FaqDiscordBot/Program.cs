using Discord;
using Discord.WebSocket;
using FaqDiscordBot.Abstractions;
using FaqDiscordBot.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Providers.Azure;
using FaqDiscordBot.Providers.Local;
using Lucene.Net.Analysis.De;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace FaqDiscordBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Bot
                    services.Configure<BotOptions>(hostContext.Configuration.GetSection("Bot"));

                    // QnA
                    //services.Configure<QnaMakerOptions>(hostContext.Configuration.GetSection("QnaMaker"));
                    //services.AddHttpClient<IFaqService, QnaMakerFaqService>(x => 
                    //    x.BaseAddress = new Uri(hostContext.Configuration.GetConnectionString("QnaServiceEndpoint")));

                    // Lucene
                    services.AddSingleton<GermanAnalyzer>(_ => new GermanAnalyzer(LuceneVersion.LUCENE_48));
                    services.AddSingleton<IndexWriterConfig>(sp =>
                        new IndexWriterConfig(LuceneVersion.LUCENE_48, sp.GetRequiredService<GermanAnalyzer>()));
                    services.AddSingleton<IndexWriter>(sp => new IndexWriter(
                        FSDirectory.Open(Path.Combine(Environment.CurrentDirectory, "index")),
                        sp.GetRequiredService<IndexWriterConfig>()));
                    services.AddTransient<IndexSearcher>(sp =>
                        new IndexSearcher(sp.GetRequiredService<IndexWriter>().GetReader(true)));
                    services.AddSingleton<IFaqService, LuceneFaqService>();
                    services.AddHostedService<LuceneWorker>();

                    // Discord
                    services.AddSingleton<DiscordSocketConfig>();
                    services.AddSingleton<DiscordSocketClient>(sp =>
                    {
                        var gate = new ManualResetEvent(false);
                        
                        var client = new DiscordSocketClient(sp.GetRequiredService<DiscordSocketConfig>());
                        client.Connected += () => Task.FromResult(gate.Set());

                        Task.WaitAll(client.LoginAsync(TokenType.Bot,
                            hostContext.Configuration.GetConnectionString("DiscordBotToken")),
                            client.StartAsync());

                        gate.WaitOne();
                        return client;
                    });

                    // Workers
                    services.AddHostedService<Worker>();
                });
    }
}
