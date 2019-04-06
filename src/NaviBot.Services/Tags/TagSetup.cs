using Microsoft.Extensions.DependencyInjection;
using NaviBot.Data.Repositories;

namespace NaviBot.Services.Tags
{
    /// <summary>
    /// Contains extension methods for configuring the tags feature upon application startup.
    /// </summary>
    public static class TagSetup
    {
        /// <summary>
        /// Adds the services and classes that make up the tags feature to a service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the tag services are to be added.</param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddTags(this IServiceCollection services)
            => services
                .AddScoped<ITagService, TagService>()
                .AddScoped<ITagRepository, TagRepository>();
    }
}
