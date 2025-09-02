using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Orders.Application.Interfaces.Services;

namespace Orders.Infrastructure.Services;

public class ProductReadService(IMongoDatabase database) : IProductReadService
{
    private readonly IMongoCollection<ProductDocument> _productCollection = database.GetCollection<ProductDocument>("products");

    public async Task<bool> ExistsAsync(long productId, CancellationToken cancellationToken = default)
    {
        return await _productCollection.Find(p => p.Id == productId).FirstOrDefaultAsync(cancellationToken) != null;
    }
    
    public async Task<bool> AddAsync(long productId, CancellationToken cancellationToken = default)
    {
        var product = new ProductDocument { Id = productId };
        await _productCollection.InsertOneAsync(product, cancellationToken: cancellationToken);
        return true;
    }
    
    public async Task<bool> DeleteAsync(long productId, CancellationToken cancellationToken = default)
    {
        var result = await _productCollection.DeleteOneAsync(p => p.Id == productId, cancellationToken);
        return result.DeletedCount > 0;
    }
}

/// <summary>
/// Product replica document.
/// </summary>
public class ProductDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.Int64)]
    public long Id { get; set; }
}