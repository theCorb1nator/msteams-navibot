using EzBotBuilder;
using EzBotBuilder.Cards;
using EzBotBuilder.Commands;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace msteams.commandbot
{
    public class CommandHandlingService : IExternalCommandService
    {
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _commands.CommandExecuted += CommandExecutedAsync;
            _services = services;
        }

         public async Task CommandExecutedAsync(Optional<CommandInfo> command, ITurnContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was succesful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.SendActivityAsync("failed");
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }
    }
}