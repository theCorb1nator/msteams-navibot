using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NaviBot.Data.Models.Core
{
    [Table("Teams")]
    public class TeamEntity
    {
        /// <summary>
        /// The tag's unique identifier.
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string TeamId { get; set; }

        public string TeamName { get; set; }
    }

}
