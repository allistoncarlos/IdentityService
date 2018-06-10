using System;
using MongoDB.Driver;

namespace IdentityService.Repository
{
    public class MongoDbContext : IMongoDbContext
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public IMongoDatabase Database { get; }
        
        public MongoDbContext(string connectionString, string databaseName)
        {
            try
            {
                ConnectionString = connectionString;
                DatabaseName = databaseName;

                var settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));

                var mongoClient = new MongoClient(settings);
                Database = mongoClient.GetDatabase(DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to reach server", ex);
            }
        }
    }
}