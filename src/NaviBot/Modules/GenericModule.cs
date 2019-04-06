using EzBotBuilder.Commands;
using Microsoft.Bot.Builder;
using System.Threading.Tasks;

namespace Teams.CommandBot.Modules
{
    public class Module : ModuleBase<ITurnContext>
    {
        [Command("ping")]
        [Alias("pong", "hello")]
        public Task PingAsync()
            => ReplyAsync("pong!");

    }

}
