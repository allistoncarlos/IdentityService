using System.Collections.Generic;

namespace IdentityService.IdentityServerProviders.Interfaces
{
	public interface IProviderRepository
	{
		IEnumerable<Provider> Get();
	}
}