using EzBotBuilder.Commands;
using Microsoft.Bot.Builder.Teams;
using NaviBot.Data.Models.Tags;
using NaviBot.Services.Tags;
using System.Threading.Tasks;

namespace Basic_Commands_Sample.Modules
{
    [Name("Tags")]
    [Summary("Use and maintain tags.")]
    [Group("tag")]
    [Alias("tags")]
    public class TagModule : ModuleBase
    {
        protected ITagService TagService { get; }

        public TagModule(ITagService tagService)
        {
            TagService = tagService;
        }

        [Command("create")]
        [Alias("add")]
        [Summary("Creates a new tag.")]
        public async Task CreateTagAsync(
             [Summary("The name that will be used to invoke the tag.")]
                string name,
             [Remainder]
            [Summary("The message that will be displayed when the tag is invoked.")]
                string content)
        {
            //ITeamsContext teamsContext = Context.TurnState.Get<ITeamsContext>();
            await TagService.CreateTagAsync(Context, name, content);
            //await Context.AddConfirmation();
        }

        [Command]
        [Priority(-10)]
        [Summary("Invokes a tag so that the message associated with the tag will be displayed.")]
        public async Task UseTagAsync(
           [Summary("The name that will be used to invoke the tag.")]
                string name)
        {
            //ITeamsContext teamsContext = Context.TurnState.Get<ITeamsContext>();
            await TagService.UseTagAsync(Context, name);
        }

        [Command("update")]
        [Alias("edit", "modify")]
        [Summary("Updates the contents of a tag.")]
        public async Task ModifyTagAsync(
           [Summary("The name that is used to invoke the tag.")]
                string name,
           [Remainder]
            [Summary("The new message that will be displayed when the tag is invoked.")]
                string newContent)
        {
            //ITeamsContext teamsContext = Context.TurnState.Get<ITeamsContext>();
            await TagService.ModifyTagAsync(Context, name, newContent);
            //await Context.AddConfirmation();
        }

        [Command("delete")]
        [Alias("remove")]
        [Summary("Deletes a tag.")]
        public async Task DeleteTagAsync(
           [Summary("The name that is used to invoke the tag.")]
                string name)
        {
            //ITeamsContext teamsContext = Context.TurnState.Get<ITeamsContext>();
            await TagService.DeleteTagAsync(Context, name);
            //await Context.AddConfirmation();
        }

        [Command("all")]
        [Alias("list all")]
        [Summary("Lists all tags available in the current guild.")]
        public async Task ListAllAsync()
        {
            ITeamsContext teamsContext = Context.TurnState.Get<ITeamsContext>();

            var tags = await TagService.GetSummariesAsync(new TagSearchCriteria()
            {
                TeamId = teamsContext.Team.Id,          
            });

            //TODO Add embeds var embed = await BuildEmbedAsync(tags, ownerGuild: Context.Guild);
            await ReplyAsync("coming soon");
            //await ReplyAsync(embed);
        }
    }
}
