using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Nito.AsyncEx;
using System;
using System.Threading.Tasks;

namespace NaviBot.Data.Repositories
{
    public class RepositoryTransactionFactory
    {
        public async Task<IRepositoryTransaction> BeginTransactionAsync(DatabaseFacade database)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            return new RepositoryTransaction(
                (database.CurrentTransaction is null)
                    ? await database.BeginTransactionAsync()
                    : null,
                await _lockProvider.LockAsync());
        }
        private AsyncLock _lockProvider { get; }
           = new AsyncLock();

        private class RepositoryTransaction : IRepositoryTransaction
        {
            public RepositoryTransaction(IDbContextTransaction transaction, IDisposable @lock)
            {
                _transaction = transaction;
                _lock = @lock;
            }

            public void Commit()
            {
                if (!_hasCommitted)
                {
                    _transaction?.Commit();
                    _hasCommitted = true;
                }
            }

            public void Dispose()
            {
                if (!_hasDisposed)
                {
                    if (!_hasCommitted)
                        _transaction?.Rollback();

                    _lock.Dispose();

                    _hasDisposed = true;
                }
            }

            private bool _hasCommitted
                = false;

            private bool _hasDisposed
                = false;

            private readonly IDbContextTransaction _transaction;

            private readonly IDisposable _lock;
        }
    }
}