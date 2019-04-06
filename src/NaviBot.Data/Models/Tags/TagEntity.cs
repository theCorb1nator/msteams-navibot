using Microsoft.EntityFrameworkCore;
using NaviBot.Data.Models.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaviBot.Data.Models.Tags
{
    [Table("Tags")]
    public class TagEntity
    {
        /// <summary>
        /// The tag's unique identifier.
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The unique identifier for the team in which the tag was created
        /// </summary>     
        public string TeamId { get; set; }


        /// <summary>
        /// The unique identifier of the action that created the tag.
        /// </summary>
        [Required]
        [ForeignKey(nameof(CreateAction))]
        public long CreateActionId { get; set; }

        /// <summary>
        /// The action that created the tag.
        /// </summary>
        public virtual TagActionEntity CreateAction { get; set; }

        /// <summary>
        /// The unique identifier of the action that deleted the tag.
        /// </summary>
        [ForeignKey(nameof(DeleteAction))]
        public long? DeleteActionId { get; set; }

        /// <summary>
        /// The action that deleted the tag.
        /// </summary>
        public virtual TagActionEntity DeleteAction { get; set; }

        /// <summary>
        /// The unique string that is used to invoke the tag.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The message that will be displayed when the tag is invoked.
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// The Teams ChannelAccount ID of the user to whom the tag belongs, if any.
        /// </summary>
        public string OwnerUserId { get; set; }

        /// <summary>
        /// The user to whom the tag belongs, if any.
        /// </summary>
        public virtual TeamUserEntity OwnerUser { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<TagEntity>()
                .HasOne(x => x.CreateAction)
                .WithOne()
                .HasForeignKey<TagEntity>(x => x.CreateActionId);

            modelBuilder
                .Entity<TagEntity>()
                .HasOne(x => x.DeleteAction)
                .WithOne()
                .HasForeignKey<TagEntity>(x => x.DeleteActionId);

        }
    }
}
