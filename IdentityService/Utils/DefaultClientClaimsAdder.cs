using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Utils
{
    public class DefaultClientClaimsAdder : ICustomTokenRequestValidator
    {
        private readonly IConfiguration configuration;

        public DefaultClientClaimsAdder(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            var client = context.Result.ValidatedRequest.Client;

            if (!client.AllowedGrantTypes.Contains(GrantType.ClientCredentials))
                return Task.CompletedTask;

            if (!Convert.ToBoolean(configuration["APIEnableScopePerClaim"])) return Task.CompletedTask;

            if (string.IsNullOrEmpty(configuration["APIClientScopes"])) return Task.CompletedTask;

            var scopes = configuration["APIClientScopes"]?.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var scope in scopes)
            {
                client.Claims.Add(new Claim(scope, "true"));
            }

            return Task.CompletedTask;
        }
    }
}
