using System;
using System.Collections.Generic;
using System.Text;

namespace NaviBot.Data.Models.Tags
{
    public class TagCreationData
    {
        public string TeamId { get; set; }

        public string CreatedById { get; set; }

        /// <summary>
        /// See <see cref="TagEntity.Name"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// See <see cref="TagEntity.Content"/>.
        /// </summary>
        public string Content { get; set; }

        internal TagEntity ToEntity()
           => new TagEntity()
           {
               CreateAction = new TagActionEntity()
               {
                   Created = DateTimeOffset.Now,
                   Type = TagActionType.TagCreated,
                   CreatedById = CreatedById,
               },
               Name = Name.ToLower(),
               Content = Content,
               OwnerUserId = CreatedById,
           };
    }
}
