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