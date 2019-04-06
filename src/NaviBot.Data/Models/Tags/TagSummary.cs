using NaviBot.Data.ExpandableQueries;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NaviBot.Data.Models.Tags
{
    public class TagSummary
    {
        /// <summary>
        /// See <see cref="TagEntity.Id"/>.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// See <see cref="TagEntity.TeamId"/>.
        /// </summary>
        public string TeamId { get; set;}

        /// <summary>
        /// See <see cref="TagEntity.CreateAction"/>.
        /// </summary>
        public TagActionBrief CreateAction { get; set; }

        /// <summary>
        /// See <see cref="TagEntity.DeleteAction"/>.
        /// </summary>
        public TagActionBrief DeleteAction { get; set; }

        /// <summary>
        /// See <see cref="TagEntity.Name"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// See <see cref="TagEntity.Content"/>.
        /// </summary>
        public string Content { get; set; }

        [ExpansionExpression]
        internal static readonly Expression<Func<TagEntity, TagSummary>> FromEntityProjection
            = entity => new TagSummary()
            {
                Id = entity.Id,
                TeamId = entity.TeamId,
                CreateAction = entity.CreateAction.Project(TagActionBrief.FromEntityProjection),
                DeleteAction = (entity.DeleteAction == null)
                    ? null
                    : entity.DeleteAction.Project(TagActionBrief.FromEntityProjection),
                Name = entity.Name,
                Content = entity.Content
            };
    }
}
