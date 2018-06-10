using IdentityServer4.Models;
using IdentityService.Domain;
using MongoDB.Bson;

namespace IdentityService.Models
{
    public class IdentityServicePersistedGrant : PersistedGrant, IEntity
    {
        public ObjectId Id { get; set; }

        public IdentityServicePersistedGrant()
        {
            
        }
        
        public IdentityServicePersistedGrant(PersistedGrant grant)
        {
            ClientId = grant.ClientId;
            CreationTime = grant.CreationTime;
            Data = grant.Data;
            Expiration = grant.Expiration;
            Key = grant.Key;
            SubjectId = grant.SubjectId;
            Type = grant.Type;
        }
    }
}