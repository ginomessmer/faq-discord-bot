using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using QnaMakerDiscordBot.Abstractions;
using QnaMakerDiscordBot.Azure;
using QnaMakerDiscordBot.Options;

namespace QnaMakerDiscordBot
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
                    services.Configure<QnaMakerOptions>(hostContext.Configuration.GetSection("QnaMaker"));
                    services.AddHttpClient<IFaqService, QnaMakerServiceClient>(x => 
                        x.BaseAddress = new Uri(hostContext.Configuration.GetConnectionString("QnaServiceEndpoint")));

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
