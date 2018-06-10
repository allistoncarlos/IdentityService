using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityService.Repository;

namespace IdentityService.IdentityServices
{
	public class PersistedGrantStore : IPersistedGrantStore
	{
		protected IRepository<string, IdentityServicePersistedGrant> DbRepository;

		public PersistedGrantStore(IRepository<string, IdentityServicePersistedGrant> repository)
		{
			DbRepository = repository;
		}

		public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
		{
			return Task.Run(async () =>
			{
				var result = await DbRepository.Load(i => i.SubjectId == subjectId);
				return result.Select(x => x as PersistedGrant).AsEnumerable();
			});

		}

		public Task<PersistedGrant> GetAsync(string key)
		{
			return Task.Run(async () =>
			{
				var result = await DbRepository.Get(i => i.Key == key);
				return (PersistedGrant)result;
			});

		}

		public Task RemoveAllAsync(string subjectId, string clientId)
		{
			return Task.Run(async () =>
			{
				var persistedGrant = await DbRepository.Get(i => i.SubjectId == subjectId && i.ClientId == clientId);

				await DbRepository.Delete(persistedGrant.Id.ToString());
			});
		}

		public Task RemoveAllAsync(string subjectId, string clientId, string type)
		{

			return Task.Run(async () =>
			{
				var persistedGrant = await DbRepository.Get(i => i.SubjectId == subjectId && i.ClientId == clientId && i.Type == type);

				await DbRepository.Delete(persistedGrant.Id.ToString());
			});
		}

		public Task RemoveAsync(string key)
		{
			return Task.Run(async () =>
			{
				var persistedGrant = await DbRepository.Get(i => i.Key == key);

				await DbRepository.Delete(persistedGrant.Id.ToString());
			});
		}

		public Task StoreAsync(PersistedGrant grant)
		{
			return Task.Run(async () =>
			{
				await DbRepository.Save(new IdentityServicePersistedGrant(grant));
			});
		}
	}
}