using EzBotBuilder.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NaviBot.Data;
using System;
using System.Linq;

namespace msteams.commandbot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var conString = Configuration["connectionString"];
            services.AddDbContext<NaviBotContext>(opt =>
            {
                opt.UseNpgsql(conString);
            });

            services.AddNaviBot();
            services.AddSingleton<CommandService>()
            .AddSingleton<CommandHandlingService>();
           

           
            services.AddBot<MyBot>(options =>
            {
                var secretKey = Configuration.GetSection("botFileSecret")?.Value;

                // Loads .bot configuration file and adds a singleton that your Bot can access through dependency injection.

                var botConfig = BotConfiguration.Load(@".\navibot.bot");
                services.AddSingleton(sp => botConfig);
             



                // Retrieve current endpoint.

                var service = botConfig.Services.Where(s => s.Type == "endpoint" && s.Name == "development").FirstOrDefault();
                if (!(service is EndpointService endpointService))
                {
                    throw new InvalidOperationException($"The .bot file does not contain a development endpoint.");
                }

                options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);
                // Catches any errors that occur during a conversation turn and logs them.
                options.OnTurnError = async (context, exception) =>
                {
                    await context.SendActivityAsync("Sorry, it looks like something went wrong.");
                };
            });
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseBotFramework();
        }
    }
}
