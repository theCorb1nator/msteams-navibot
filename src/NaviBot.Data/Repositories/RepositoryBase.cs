namespace NaviBot.Data.Repositories
{
    public abstract class RepositoryBase
    {
        internal protected NaviBotContext NaviBotContext { get; }

        public RepositoryBase(NaviBotContext naviBotContext)
        {
            NaviBotContext = naviBotContext;
        }          
    }
}
