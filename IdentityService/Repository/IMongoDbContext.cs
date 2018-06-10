using MongoDB.Driver;

namespace IdentityService.Repository
{
    public interface IMongoDbContext
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }

        IMongoDatabase Database { get; }
    }
}