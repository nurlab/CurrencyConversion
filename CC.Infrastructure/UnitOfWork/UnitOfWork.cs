using CC.Application.Interfaces;
using CC.Infrastructure.DatabaseContext;

namespace CC.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private bool disposed = false;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (!disposed)
            {
                if (_dbContext != null)
                {
                    await _dbContext.DisposeAsync();
                }

                disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext?.Dispose();
                }
                disposed = true;
            }
        }

    }
}
