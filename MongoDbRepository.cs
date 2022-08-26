using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Classes;

public class MongoDbRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
{
    IMongoDatabase mongoDatabase;
    private IMongoCollection<TEntity> collection;

    public MongoDbRepository(string ConnectionString, string DatabaseName, string CollectionName)
    {
        GetDatabase(ConnectionString, DatabaseName);
        GetCollection(CollectionName);
    }

    public async Task<List<TEntity>> GetAsync() =>
        await collection.Find(_ => true).ToListAsync();

    public async Task<TEntity> GetAsync(string id) =>
        await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(TEntity entity) =>
        await collection.InsertOneAsync(entity);

    public async Task CreateAsync(List<TEntity> entities) =>
        await collection.InsertManyAsync(entities);

    public async Task ClearAndCreateAsync(List<TEntity> entities)
    {
        await collection.DeleteManyAsync(_ => true);
        await collection.InsertManyAsync(entities);
    }

    public async Task UpdateAsync(string id, TEntity updatedEntity) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedEntity);

    public async Task RemoveAsync() =>
        await collection.DeleteManyAsync(_ => true);

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);

    #region Private Helper Methods  
    private void GetDatabase(string connectionString, string databaseName)
    {
        var mongoClient = new MongoClient(connectionString);
        mongoDatabase = mongoClient.GetDatabase(databaseName);
    }

    //private string GetConnectionString()
    //{
    //    return "";
    //    //ConfigurationManager
    //    //.AppSettings
    //    //.Get(“MongoDbConnectionString”)
    //    //.Replace("{ DB_NAME}", GetDatabaseName());
    //}

    //private string GetDatabaseName()
    //{
    //    return "";
    //    //ConfigurationManager
    //    //.AppSettings
    //    //.Get("MongoDbDatabaseName");
    //}

    private void GetCollection(string CollectionName)
    {
        collection = mongoDatabase.GetCollection<TEntity>(CollectionName);
    }
    #endregion
}