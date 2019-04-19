using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NaviBot.Data;
using System;
using System.Threading.Tasks;

namespace msteams.commandbot
{
    public class Program
    {
        static void Main(string[] args)
                  => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string [] args)
        {

       
            var host = CreateWebHostBuilder(args).Build();
            await host.Services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    services.GetRequiredService<NaviBotContext>();
                    //.Database.Migrate();
                    // Use the context here
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred.");
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, logging) =>
        {
            // Add Azure Logging
            logging.AddAzureWebAppDiagnostics();
        })
        .UseStartup<Startup>();
    }
}
