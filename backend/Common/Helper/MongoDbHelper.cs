using MongoDB.Bson;
using MongoDB.Driver;

namespace Common.Helper;

public class MongoDbHelper
{
    private readonly MongoClient _client;
    public MongoDbHelper(string connectionString)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        _client = new MongoClient(settings);
    }

    public IMongoDatabase GetDatabase(string databaseName)
    {
        return _client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName)
    {
        return _client.GetDatabase(databaseName).GetCollection<T>(collectionName);
    }
}

public static class MongoDbExtension
{
    public static async Task CreateAsync<T>(this IMongoCollection<T> collection, T document)
    {
        await collection.InsertOneAsync(document);
    }

    public static async Task<List<T>> GetAllDocAsync<T>(this IMongoCollection<T> collection, FilterDefinition<T>? filter = null)
    {
        filter ??= Builders<T>.Filter.Empty;
        return await collection.Find(filter).ToListAsync();
    }

    public static async Task<T> GetByIdAsync<T, TId>(this IMongoCollection<T> collection, TId id)
    {
        var filter = GetIdFilterDefinition<T, TId>(id);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public static async Task<UpdateResult> UpdateDocumentAsync<T, TId>(this IMongoCollection<T> collection, TId id, T updatedEntity)
    {
        var filter = GetIdFilterDefinition<T, TId>(id);
        var updateBuilder = Builders<T>.Update;
        var updateDefinition = updateBuilder.Combine(updatedEntity!.GetType().GetProperties().Select(p => updateBuilder.Set(p.Name, p.GetValue(updatedEntity))));
        return await collection.UpdateOneAsync(filter, updateDefinition);
    }

    public static async Task<DeleteResult> DeleteAsync<T, TId>(this IMongoCollection<T> collection, TId id)
    {
        var filter = GetIdFilterDefinition<T, TId>(id);
        return await collection.DeleteOneAsync(filter);
    }

    public static async Task<List<T>> FindByConditionAsync<T>(this IMongoCollection<T> collection, string field, string value)
    {
        var filter = Builders<T>.Filter.Eq(field, value);
        return await collection.Find(filter).ToListAsync();
    }

    private static FilterDefinition<T> GetIdFilterDefinition<T, TId>(TId id)
    {
        FilterDefinition<T> filter;
        if (id is ObjectId objectId)
        {
            filter = Builders<T>.Filter.Eq("_id", objectId);
        }
        else if (id is string strId)
        {
            filter = Builders<T>.Filter.Eq("_id", strId);
        }
        else if (id is Guid guidId)
        {
            filter = Builders<T>.Filter.Eq("_id", guidId.ToString());
        }
        else
        {
            throw new NotSupportedException($"Unsupported ID type: {typeof(TId).Name}");
        }

        return filter;
    }
}
