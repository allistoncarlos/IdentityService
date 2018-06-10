using System.Collections.Generic;

namespace IdentityService.IdentityServerProviders
{
	public class ProviderDataSource
	{
		public static IEnumerable<Provider> GetProviders()
		{
			return new List<Provider>
			{
				new Provider
				{
					ProviderId = 1,
					Name = "Facebook",
					UserInfoEndPoint = "https://graph.facebook.com/v2.11/me"
				},
				new Provider
				{
					ProviderId = 2,
					Name = "Google",
					UserInfoEndPoint = "https://www.googleapis.com/oauth2/v2/userinfo"
				}
			};
		}
	}
}