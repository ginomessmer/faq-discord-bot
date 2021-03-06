using Discord;
using Discord.WebSocket;
using FaqDiscordBot.Infrastructure;
using FaqDiscordBot.Options;
using FaqDiscordBot.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using FaqDiscordBot.Workers;

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
                    
                    services.AddQnaMakerFaqProvider(hostContext.Configuration.GetSection("QnaMaker"),
                        hostContext.Configuration.GetConnectionString("QnaServiceEndpoint"));

                    // DB
                    services.AddDbContext<FaqDbContext>(x =>
                        x.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultDbContext")));

                    services.AddMemoryCache();
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
                    services.AddHostedService<BotInitializeWorker>();
                    services.AddHostedService<DmListenerWorker>();
                    services.AddHostedService<KnowledgeBaseUpdateWorker>();
                    services.AddHostedService<InitializeDatabaseWorker>();
                    services.AddHostedService<DanglingQuestionsAutoPurgeWorker>();
                    services.AddHostedService<DanglingQuestionsReminderWorker>();

                    // Telemetry
                    services.AddApplicationInsightsTelemetryWorkerService();
                });
    }
}
