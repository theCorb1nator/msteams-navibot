using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NaviBot.Data.Models.Core
{
    [Table("Users")]
    public class TeamUserEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// The user id for the user or bot on the channel(e.g. joe@example.com)
        /// </summary>
        public string ChannelAccountId { get; set; }

        /// <summary>
        /// The friendly name of the user 
        /// </summary>
        public string Name { get; set; }

        public string Role { get; set; }

    }
}
