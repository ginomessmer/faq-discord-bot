using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

                    services.AddHttpClient<QnaMakerServiceClient>(x => 
                        x.BaseAddress = new Uri(hostContext.Configuration.GetConnectionString("QnaServiceEndpoint")));
                    
                    services.AddHostedService<Worker>();
                });
    }
}
