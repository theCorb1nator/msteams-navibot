using System;
using System.Collections.Generic;
using System.Text;

namespace NaviBot.Data.Models.Tags
{
    /// <summary>
    /// Describes an operation to modify a tag.
    /// </summary>
    public class TagMutationData
    {
        /// <summary>
        /// See <see cref="TagEntity.TeamId"/>.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// See <see cref="TagEntity.Name"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// See <see cref="TagEntity.Content"/>.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// See <see cref="TagEntity.OwnerUserId"/>.
        /// </summary>
        public string OwnerUserId { get; set; }

        internal static TagMutationData FromEntity(TagEntity entity)
            => new TagMutationData()
            {
                TeamId = entity.TeamId,
                Name = entity.Name,
                Content = entity.Content,
                OwnerUserId = entity.OwnerUserId,
            };

        internal TagEntity ToEntity()
            => new TagEntity()
            {
                TeamId = TeamId,
                Name = Name,
                Content = Content,
                OwnerUserId = OwnerUserId,
            };
    }
}
