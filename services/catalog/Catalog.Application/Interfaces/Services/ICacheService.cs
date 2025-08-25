namespace Catalog.Application.Interfaces.Services;

/// <summary>
///     Interface for cache service operations.
/// </summary>
public interface ICacheService
{
    /// <summary>
    ///     Retrieves a cached value by key.
    /// </summary>
    /// <typeparam name="T">Type of the cached value.</typeparam>
    /// <param name="key">Cache key.</param>
    /// <returns>The cached value or null if not found.</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    ///     Sets a value in the cache with an optional expiration.
    /// </summary>
    /// <typeparam name="T">Type of the value to cache.</typeparam>
    /// <param name="key">Cache key.</param>
    /// <param name="value">Value to cache.</param>
    /// <param name="expiration">Optional expiration time.</param>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

    /// <summary>
    ///     Removes a value from the cache by key.
    /// </summary>
    /// <param name="key">Cache key.</param>
    Task RemoveAsync(string key);
}