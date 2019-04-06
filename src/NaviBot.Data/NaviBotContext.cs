using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NaviBot.Data.Models.Core;
using NaviBot.Data.Models.Tags;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NaviBot.Data
{
    public class NaviBotContext : DbContext
    {
        public NaviBotContext(DbContextOptions<NaviBotContext> options) : base(options)
        {
            Debugger.Launch();
        }

        // For building fakes during testing
        public NaviBotContext()
        {
        }

        public DbSet<TeamUserEntity> Users { get; set; }

        public DbSet<TeamEntity> Teams { get; set; }

        public DbSet<TagEntity> Tags { get; set; }

        public DbSet<TagActionEntity> TagActions { get; set; }

    }

}
