using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using IdentityService.IdentityServerProviders.Interfaces;
using Microsoft.AspNetCore.Identity.MongoDB;
using Newtonsoft.Json.Linq;

namespace IdentityService.IdentityServerProviders
{
	public class GoogleAuthProvider<TUser> : IGoogleAuthProvider where TUser : IdentityUser, new()
	{

		private readonly IProviderRepository providerRepository;
		private readonly HttpClient httpClient;
		public GoogleAuthProvider(

			IProviderRepository providerRepository,
			HttpClient httpClient
		)
		{

			this.providerRepository = providerRepository;
			this.httpClient = httpClient;
		}
		public Provider Provider => providerRepository.Get()
			.FirstOrDefault(x => x.Name.ToLower() == ProviderType.Google.ToString().ToLower());
		public JObject GetUserInfo(string accessToken)
		{
		    var request = new Dictionary<string, string> {{"token", accessToken}};

		    var result = httpClient.GetAsync(Provider.UserInfoEndPoint + QueryBuilder.GetQuery(request, ProviderType.Google)).Result;

		    if (!result.IsSuccessStatusCode) return null;

		    var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
		    return infoObject;
		}
	}
}