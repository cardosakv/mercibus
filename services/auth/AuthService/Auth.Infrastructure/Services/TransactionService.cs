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
            await dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await dbContext.Database.RollbackTransactionAsync();
        }
    }
}
