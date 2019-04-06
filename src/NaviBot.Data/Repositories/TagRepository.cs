using Microsoft.EntityFrameworkCore;
using NaviBot.Data.ExpandableQueries;
using NaviBot.Data.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviBot.Data.Repositories
{
    public interface ITagRepository
    {
        Task<IRepositoryTransaction> BeginMaintainTransactionAsync();

        Task<IRepositoryTransaction> BeginUseTransactionAsync();

        Task<long> CreateAsync(TagCreationData data);

        Task<TagSummary> ReadSummaryAsync(string teamId, string name);

        Task<IReadOnlyCollection<TagSummary>> SearchSummariesAsync(TagSearchCriteria searchCriteria);

        Task<bool> TryModifyAsync(string teamId, string name, string modifiedByUserId, Action<TagMutationData> modifyAction);

        Task<bool> TryDeleteAsync(string teamId, string name, string deletedByUserId);
    }

    public class TagRepository : RepositoryBase, ITagRepository
    {
        private static readonly RepositoryTransactionFactory _maintainTransactionFactory
            = new RepositoryTransactionFactory();

        private static readonly RepositoryTransactionFactory _useTransactionFactory
            = new RepositoryTransactionFactory();

        public TagRepository(NaviBotContext naviBotContext)
            : base(naviBotContext)
        {

        }

        public Task<IRepositoryTransaction> BeginMaintainTransactionAsync()
           => _maintainTransactionFactory.BeginTransactionAsync(NaviBotContext.Database);

        public Task<IRepositoryTransaction> BeginUseTransactionAsync()
           => _useTransactionFactory.BeginTransactionAsync(NaviBotContext.Database);

        /// <inheritdoc />
        public async Task<long> CreateAsync(TagCreationData data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            var entity = data.ToEntity();

            await NaviBotContext.Tags.AddAsync(entity);
            await NaviBotContext.SaveChangesAsync();

            entity.CreateAction.NewTagId = entity.Id;
            await NaviBotContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<TagSummary> ReadSummaryAsync(string teamId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The supplied name cannot be null or whitespace.", nameof(name));

            return await NaviBotContext.Tags.AsNoTracking()
                .Where(x
                    => x.TeamId == teamId
                    && x.Name == name.ToLower()
                    && x.DeleteActionId == null)
                .AsExpandable()
                .Select(TagSummary.FromEntityProjection)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<TagSummary>> SearchSummariesAsync(TagSearchCriteria searchCriteria)
        {
            if (searchCriteria is null)
                throw new ArgumentNullException(nameof(searchCriteria));

            return await NaviBotContext.Tags.AsNoTracking()
                .Where(x => x.DeleteActionId == null)
                .FilterBy(searchCriteria)
                .OrderBy(x => x.Name)
                .AsExpandable()
                .Select(TagSummary.FromEntityProjection)
                .ToArrayAsync();
        }


        public async Task<bool> TryDeleteAsync(string teamId, string name, string deletedByUserId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The supplied name cannot be null or whitespace.", nameof(name));

            var entity = await NaviBotContext.Tags.FirstOrDefaultAsync(x
                => x.TeamId == teamId
                && x.Name == name.ToLower()
                && x.DeleteActionId == null);

            if (entity is null)
                return false;

            var deleteAction = new TagActionEntity()
            {
                TeamId = entity.TeamId,
                Created = DateTimeOffset.Now,
                Type = TagActionType.TagDeleted,
                CreatedById = deletedByUserId,
                OldTagId = entity.Id,
            };

            await NaviBotContext.TagActions.AddAsync(deleteAction);
            await NaviBotContext.SaveChangesAsync();

            entity.DeleteActionId = deleteAction.Id;
            await NaviBotContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TryModifyAsync(string teamId, string name, string modifiedByUserId, Action<TagMutationData> modifyAction)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The supplied name cannot be null or whitespace.", nameof(name));

            if (modifyAction is null)
                throw new ArgumentNullException(nameof(modifyAction));

            var oldTag = await NaviBotContext.Tags.FirstOrDefaultAsync(x
                => x.TeamId == teamId
                && x.Name == name.ToLower()
                && x.DeleteActionId == null);

            if (oldTag is null)
                return false;

            var action = new TagActionEntity()
            {
                TeamId = teamId,
                Type = TagActionType.TagModified,
                Created = DateTimeOffset.Now,
                CreatedById = modifiedByUserId,
                OldTagId = oldTag.Id,
            };

            await NaviBotContext.TagActions.AddAsync(action);
            await NaviBotContext.SaveChangesAsync();

            var newTagData = TagMutationData.FromEntity(oldTag);
            modifyAction(newTagData);

            var newTag = newTagData.ToEntity();
            newTag.CreateActionId = action.Id;

            await NaviBotContext.Tags.AddAsync(newTag);
            await NaviBotContext.SaveChangesAsync();

            action.NewTagId = newTag.Id;
            oldTag.DeleteActionId = action.Id;

            await NaviBotContext.SaveChangesAsync();

            return true;
        }
    }
}
