using MongoDB.Bson;

namespace IdentityService.Domain
{
    public abstract class Entity : IEntity
    {
        public ObjectId Id { get; set; }

        public Entity()
        {

        }

        public Entity(string id = "")
        {
            if (!string.IsNullOrEmpty(id))
                Id = ObjectId.Parse(id);
        }
    }
}
