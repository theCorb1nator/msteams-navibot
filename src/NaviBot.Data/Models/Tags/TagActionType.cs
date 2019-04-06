namespace NaviBot.Data.Models.Tags
{
    public enum TagActionType
    {
        /// <summary>
        /// Describes an action where a tag was created.
        /// </summary>
        TagCreated,
        /// <summary>
        /// Describes an action where a tag was modified.
        /// </summary>
        TagModified,
        /// <summary>
        /// Describes an action where a tag was deleted.
        /// </summary>
        TagDeleted,
    }
}