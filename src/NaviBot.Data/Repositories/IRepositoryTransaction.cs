using System;

namespace NaviBot.Data.Repositories
{
    public interface IRepositoryTransaction : IDisposable
    {
            /// <summary>
            /// Commits any changes performed during the transaction to be written to the underlying data storage provider.
            /// 
            /// This should usually be called right before <see cref="IDisposable.Dispose"/>, and may only be called once per transaction.
            /// 
            /// If this method is not called before <see cref="IDisposable.Dispose"/>, all changes are automatically rolled back.
            /// </summary>
            void Commit();    
    }
}