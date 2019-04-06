using EzBotBuilder.Commands;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teams.Net.Commands;

namespace msteams.commandbot
{
    public class MyBot : IBot
    {
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public MyBot(CommandService commandService, IServiceProvider services)
        {
            _commands = commandService;
            _services = services;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var argPos = 0;
                if (!(turnContext.Activity.MentionsRecipient() || turnContext.Activity.Text.HasStringPrefix("!", ref argPos))) return;
                await _commands.ExecuteAsync(turnContext, argPos, _services);
            }
        }
    }
}