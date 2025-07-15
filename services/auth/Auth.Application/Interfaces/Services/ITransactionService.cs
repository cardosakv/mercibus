namespace Auth.Application.Interfaces.Services;

/// <summary>
/// Interface for transactions in the data access layer.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Begins the transaction.
    /// </summary>
    Task BeginAsync();

    /// <summary>
    /// Commits the changes in the transaction.
    /// </summary>
    /// <returns></returns>
    Task CommitAsync();

    /// <summary>
    /// Reverts the changes occured in the transaction.
    /// </summary>
    /// <returns></returns>
    Task RollbackAsync();
}