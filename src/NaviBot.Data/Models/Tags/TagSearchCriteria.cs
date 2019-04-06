using NaviBot.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaviBot.Data.Models.Tags
{
    public class TagSearchCriteria
    {
        public string Name { get; set; }

        public string TeamId { get; set; }

        public string OwnerUserId { get; set; }
    }

    internal static class TagSearchCriteriaExtensions
    {
        public static IQueryable<TagEntity> FilterBy(this IQueryable<TagEntity> query, TagSearchCriteria criteria)
            => query
                .FilterBy(
                    x => x.TeamId == criteria.TeamId,
                    !(criteria.TeamId is null))
                .FilterBy(
                    x => x.Name.Contains(criteria.Name.ToLower()),
                    !(criteria.Name is null))
                .FilterBy(
                    x => x.OwnerUserId == null
                        ? false
                        : x.OwnerUserId == criteria.OwnerUserId,
                    !(criteria.OwnerUserId is null));
    }
}
