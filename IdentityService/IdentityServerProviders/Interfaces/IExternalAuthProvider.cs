using Newtonsoft.Json.Linq;

namespace IdentityService.IdentityServerProviders.Interfaces
{
	public interface IExternalAuthProvider
	{
		JObject GetUserInfo(string accessToken);
	}
}