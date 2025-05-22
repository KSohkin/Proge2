using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public abstract class ServiceTestBase : IDisposable
    {
        private ApplicationDbContext _dbContext;
        private bool disposedValue;

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }

                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
                _dbContext = new ApplicationDbContext(options);
                return _dbContext;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
