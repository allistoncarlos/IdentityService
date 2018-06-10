using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityService
{
    public class IdentityConfig
    {
        public static IConfiguration Configuration { get; set; }
        
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var identityResources = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

            if(!Convert.ToBoolean(Configuration["APIEnableScopePerClaim"])) return identityResources;

            if (string.IsNullOrEmpty(Configuration["APIScopes"])) return identityResources;
            
            var scopes = Configuration["APIScopes"]?.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var scope in scopes)
            {
                identityResources.Add(new IdentityResource(scope, new List<string> { scope }));
            }

            return identityResources;
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            var resources = new List<ApiResource>
            {
                new ApiResource(Configuration["APIName"], Configuration["APIDescription"])
            };

            return resources;
        }
        
        public static IEnumerable<Client> GetClients()
        {
            var clients = new List<Client>();

            if (!string.IsNullOrEmpty(Configuration["Authentication:Swagger:ClientId"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:Swagger:ClientDescription"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:Swagger:ClientSecret"]))
            {
                var swaggerClient = new Client
                {
                    ClientId = Configuration["Authentication:Swagger:ClientId"],
                    ClientName = Configuration["Authentication:Swagger:ClientDescription"],
                    AllowedGrantTypes = new[] {GrantType.ResourceOwnerPassword, "external"},
                    ClientSecrets =
                    {
                        new Secret(Configuration["Authentication:Swagger:ClientSecret"].Sha256())
                    },

                    IdentityTokenLifetime = 3600,
                    AccessTokenLifetime = 3600,
                    AuthorizationCodeLifetime = 3600,

                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,

                    AllowedScopes = {Configuration["APIName"], "offline_access"},
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                };

                clients.Add(swaggerClient);
            }

            if (!string.IsNullOrEmpty(Configuration["Authentication:iOS:ClientId"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:iOS:AppName"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:iOS:ClientSecret"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:iOS:AppURLScheme"]))
            {
                var iOSClient = new Client
                {
                    ClientId = Configuration["Authentication:iOS:ClientId"],
                    ClientName = Configuration["Authentication:iOS:AppName"],
                    ClientSecrets =
                    {
                        new Secret(Configuration["Authentication:iOS:ClientSecret"].Sha256())
                    },
                    AllowedGrantTypes = new[] {"authorization_code"},

                    IdentityTokenLifetime = 3600,
                    AccessTokenLifetime = 3600,
                    AuthorizationCodeLifetime = 3600,

                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,

                    RedirectUris =
                    {
                        $"{Configuration["Authentication:iOS:AppURLScheme"]}"
                    },

                    AllowedScopes = {Configuration["APIName"], "offline_access"},
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                };

                clients.Add(iOSClient);
            }

            if (!string.IsNullOrEmpty(Configuration["Authentication:Android:ClientId"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:Android:AppName"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:Android:ClientSecret"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:Android:AppURLScheme"]))
            {
                var androidClient = new Client
                {
                    ClientId = Configuration["Authentication:Android:ClientId"],
                    ClientName = Configuration["Authentication:Android:AppName"],
                    ClientSecrets =
                    {
                        new Secret(Configuration["Authentication:Android:ClientSecret"].Sha256())
                    },
                    AllowedGrantTypes = new[] { "authorization_code" },

                    IdentityTokenLifetime = 3600,
                    AccessTokenLifetime = 3600,
                    AuthorizationCodeLifetime = 3600,

                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,

                    RedirectUris =
                    {
                        $"{Configuration["Authentication:Android:AppURLScheme"]}"
                    },

                    AllowedScopes = { Configuration["APIName"], "offline_access" },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                };

                clients.Add(androidClient);
            }

            if (!string.IsNullOrEmpty(Configuration["Authentication:Web:ClientId"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:Web:ClientDescription"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:Web:ClientSecret"]) &&
                !string.IsNullOrEmpty(Configuration["Authentication:Web:ClientUri"]))
            {
                var webClient = new Client
                {
                    ClientId = Configuration["Authentication:Web:ClientId"],
                    ClientName = Configuration["Authentication:Web:ClientDescription"],
                    ClientSecrets =
                    {
                        new Secret(Configuration["Authentication:Web:ClientSecret"].Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RedirectUris = {$"{Configuration["Authentication:Web:ClientUri"]}/signin-oidc"},
                    PostLogoutRedirectUris = {$"{Configuration["Authentication:Web:ClientUri"]}/signout-callback-oidc"},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        Configuration["APIName"]
                    },
                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true
                };

                clients.Add(webClient);
            }

            if (!Convert.ToBoolean(Configuration["APIEnableScopePerClaim"])) return clients;

            if (string.IsNullOrEmpty(Configuration["APIScopes"])) return clients;

            foreach (var client in clients)
            {
                var scopes = Configuration["APIScopes"]?.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                foreach (var scope in scopes)
                {
                    client.AllowedScopes.Add(scope);
                }
            }

            return clients;
        }
    }
}