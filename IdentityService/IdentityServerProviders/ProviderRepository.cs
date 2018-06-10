using System.Collections.Generic;
using IdentityService.IdentityServerProviders.Interfaces;

namespace IdentityService.IdentityServerProviders
{
	public class ProviderRepository : IProviderRepository
	{
		public IEnumerable<Provider> Get()
		{
			return ProviderDataSource.GetProviders();
		}
	}
}