using NaviBot.Services.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNaviBot(this IServiceCollection services)
        {
            services.AddTags();
            return services;
        }
    }
}
