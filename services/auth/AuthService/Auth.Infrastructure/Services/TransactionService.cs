using Auth.Application.Interfaces;

namespace Auth.Infrastructure.Services
{
    public class TransactionService(AppDbContext dbContext) : ITransactionService
    {
        public async Task BeginAsync()
        {
             await dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (dbContext.Database.CurrentTransaction is not null)
            {
                await dbContext.Database.CommitTransactionAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (dbContext.Database.CurrentTransaction is not null)
            {
                await dbContext.Database.RollbackTransactionAsync();
            }
        }
    }
}
