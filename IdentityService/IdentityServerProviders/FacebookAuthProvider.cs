using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using IdentityService.IdentityServerProviders.Interfaces;
using Microsoft.AspNetCore.Identity.MongoDB;

namespace IdentityService.IdentityServerProviders
{
	public class FacebookAuthProvider<TUser> : IFacebookAuthProvider where TUser : IdentityUser, new()
	{

		private readonly IProviderRepository providerRepository;
		private readonly HttpClient httpClient;
		public FacebookAuthProvider(
			IProviderRepository providerRepository,
			HttpClient httpClient
		)
		{
			this.providerRepository = providerRepository;
			this.httpClient = httpClient;
		}

		public Provider Provider => providerRepository.Get()
			.FirstOrDefault(x => x.Name.ToLower() == ProviderType.Facebook.ToString().ToLower());

		public JObject GetUserInfo(string accessToken)
		{
			if (Provider == null)
			{
				throw new ArgumentNullException(nameof(Provider));
			}

			var request = new Dictionary<string, string> {{"fields", "id,email,name,gender,birthday"}, {"access_token", accessToken}};

			var url = Provider.UserInfoEndPoint + QueryBuilder.GetQuery(request, ProviderType.Facebook);
			var result = httpClient.GetAsync(url).Result;

		    if (!result.IsSuccessStatusCode) return null;

		    var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
		    return infoObject;
		}
	}
}
