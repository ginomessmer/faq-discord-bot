using Discord;
using Discord.WebSocket;
using FaqDiscordBot.Options;
using FaqDiscordBot.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                .ConfigureAppConfiguration(x => x.AddUserSecrets(typeof(Program).Assembly))
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<BotOptions>(hostContext.Configuration.GetSection("Bot"));

                    var provider = hostContext.Configuration.GetValue<string>("Provider") ?? BotOptions.Providers.Default;
                    services.AddFaqProvider(hostContext.Configuration, provider);

                    // DB
                    services.AddDbContext<FaqDbContext>(x =>
                        x.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultDbContext")));

                    services.AddMediatR(typeof(Program));

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
                    services.AddSingleton(x => x.GetRequiredService<DiscordSocketClient>() as IDiscordClient);

                    // Workers
                    services.AddHostedService<Worker>();
                });
    }
}
