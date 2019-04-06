using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using NaviBot.Data.Models.Tags;
using NaviBot.Data.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaviBot.Services.Tags
{

    public interface ITagService
    {
        //Task CreateTagAsync(string teamId, string creatorId, string name, string content);

        Task CreateTagAsync(ITurnContext turnContext, string name, string content);

        Task UseTagAsync(ITurnContext turnContext, string name);

        Task ModifyTagAsync(ITurnContext turnContext, string name, string newContent);

        Task DeleteTagAsync(ITurnContext turnContext, string name);

        //Task UseTagAsync(string teamId, string channelId, string name);

       // Task ModifyTagAsync(string teamId, string modifierId, string name, string newContent);

        //Task DeleteTagAsync(string teamId, string deleterId, string name);

        Task<IReadOnlyCollection<TagSummary>> GetSummariesAsync(TagSearchCriteria criteria);

    }

    internal class TagService : ITagService
    {
        protected ITagRepository TagRepository { get; }

        public TagService(
            ITagRepository tagRepository
            )
        {
            TagRepository = tagRepository;
        }

        public async Task CreateTagAsync(ITurnContext turnContext, string name, string content)
        {
            ITeamsContext teamsContext = turnContext.TurnState.Get<ITeamsContext>();
            string teamId = teamsContext.Team.Id;
            string creatorId = turnContext.Activity.From.Id;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be blank or whitespace.", nameof(name));

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("The tag content cannot be blank or whitespace.", nameof(content));

            name = name.Trim().ToLower();

            using (var transaction = await TagRepository.BeginMaintainTransactionAsync())
            {
                var existingTag = await TagRepository.ReadSummaryAsync(teamId, name);

                if (!(existingTag is null))
                    throw new InvalidOperationException($"A tag with the name '{name}' already exists.");

                await TagRepository.CreateAsync(new TagCreationData()
                {
                    TeamId = teamId,
                    CreatedById = creatorId,
                    Name = name,
                    Content = content,
                });

                transaction.Commit();
                await turnContext.SendActivityAsync("Tag created succesfully");
            }
        }

        public async Task UseTagAsync(ITurnContext turnContext, string name)
        {
            //TODO: roles and authentication AuthorizationService.RequireClaims(AuthorizationClaim.UseTag);
            ITeamsContext teamsContext = turnContext.TurnState.Get<ITeamsContext>();
            string teamId = teamsContext.Team.Id;
            string channelId = teamsContext.Channel.Id;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be blank or whitespace.", nameof(name));

            name = name.Trim().ToLower();

            using (var transaction = await TagRepository.BeginUseTransactionAsync())
            {
                var tag = await TagRepository.ReadSummaryAsync(teamId, name);

                if (tag is null)
                    throw new InvalidOperationException($"The tag '{name}' does not exist.");

                await turnContext.SendActivityAsync(tag.Content);

                transaction.Commit();
            }
        }

        public async Task ModifyTagAsync(ITurnContext turnContext, string name, string newContent)
        {
            ITeamsContext teamsContext = turnContext.TurnState.Get<ITeamsContext>();
            string teamId = teamsContext.Team.Id;
            string modifierId = turnContext.Activity.From.Id;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be blank or whitespace.", nameof(name));

            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("The tag content cannot be blank or whitespace.", nameof(newContent));

            name = name.Trim().ToLower();

            using (var transaction = await TagRepository.BeginMaintainTransactionAsync())
            {
                var tag = await TagRepository.ReadSummaryAsync(teamId, name);

                if (tag is null)
                    throw new InvalidOperationException($"The tag '{name}' does not exist.");

                //await EnsureUserCanMaintainTagAsync(tag, modifierId);

                await TagRepository.TryModifyAsync(teamId, name, modifierId, x => x.Content = newContent);

                transaction.Commit();
            }
        }

        public async Task DeleteTagAsync(ITurnContext turnContext, string name)
        {
            ITeamsContext teamsContext = turnContext.TurnState.Get<ITeamsContext>();
            string teamId = teamsContext.Team.Id;
            string deleterId = turnContext.Activity.From.Id;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The tag name cannot be blank or whitespace.", nameof(name));

            name = name.Trim().ToLower();

            using (var transaction = await TagRepository.BeginMaintainTransactionAsync())
            {
                var tag = await TagRepository.ReadSummaryAsync(teamId, name);

                if (tag is null)
                    throw new InvalidOperationException($"The tag '{name}' does not exist.");

                //await EnsureUserCanMaintainTagAsync(tag, deleterId);

                await TagRepository.TryDeleteAsync(teamId, name, deleterId);

                transaction.Commit();
            }
        }

        public async Task<IReadOnlyCollection<TagSummary>> GetSummariesAsync(TagSearchCriteria criteria)
        {
            if (criteria is null)
                throw new ArgumentNullException(nameof(criteria));

            return await TagRepository.SearchSummariesAsync(criteria);
        }
    }
}