using System;
using System.Collections.Generic;

namespace IdentityService.IdentityServerProviders
{
	public static class QueryBuilder
	{
		public static string FacebookUserInfoQuery(List<string> fields, string token)
		{
			return "?fields=" + string.Join(",", fields) + "&access_token=" + token;
		}

		public static string GetQuery(Dictionary<string, string> values, ProviderType provider)
		{
			switch (provider)
			{
				case ProviderType.Facebook:

					try
					{
						var fields = values["fields"];
						var accessToken = values["access_token"];
						return $"?fields={fields}&access_token={accessToken}";
					}
					catch (Exception ex)
					{
						throw ex;
					}

				case ProviderType.Google:

					var googleAccessToken = values["token"];
					return $"?access_token={googleAccessToken}";

				default:
					return null;
			}
		}
	}
}