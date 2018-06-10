using MongoDB.Bson;

namespace IdentityService.Domain
{
    public interface IEntity
    {
        ObjectId Id { get; set; }
    }
}