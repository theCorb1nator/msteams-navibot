using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NaviBot.Data.Models.Core
{
    [Table("Tenants")]
    public class TenantEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        public string TenantId { get; set; }
    }
}
