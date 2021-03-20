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
using FaqDiscordBot.Providers;
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
                    services.Configure<BotOptions>(hostContext.Configuration.GetSection("Bot"));

                    var provider = hostContext.Configuration.GetValue<string>("Mode") ?? BotOptions.Modes.Default;
                    services.AddFaqProvider(hostContext.Configuration, provider);
                    
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
