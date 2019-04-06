using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaviBot.Data
{
    public class ModixContextDesignFactory : IDesignTimeDbContextFactory<NaviBotContext>
    {
        public NaviBotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NaviBotContext>();
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=NaviBotTest;User Id=X;Password=X;");
            return new NaviBotContext(optionsBuilder.Options);
        }
    }
}
