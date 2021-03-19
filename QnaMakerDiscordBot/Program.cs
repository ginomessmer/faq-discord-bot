using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using QnaMakerDiscordBot.Azure;

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
                    services.Configure<QnaMakerOptions>(hostContext.Configuration.GetSection("QnaMaker"));
                    services.AddHttpClient<IFaqService, QnaMakerServiceClient>(x => 
                        x.BaseAddress = new Uri(hostContext.Configuration.GetConnectionString("QnaServiceEndpoint")));


                    services.AddSingleton<DiscordSocketConfig>();
                    services.AddSingleton<DiscordSocketClient>(sp =>
                    {
                        var client = new DiscordSocketClient(sp.GetRequiredService<DiscordSocketConfig>());
                        Task.WaitAll(client.LoginAsync(TokenType.Bot,
                            hostContext.Configuration.GetConnectionString("DiscordBotToken")),
                            client.StartAsync());
                        
                        return client;
                    });


                    services.AddHostedService<Worker>();
                });
    }
}
