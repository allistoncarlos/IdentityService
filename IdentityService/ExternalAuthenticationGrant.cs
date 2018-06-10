using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityService.IdentityServerProviders;
using IdentityService.IdentityServerProviders.Interfaces;
using Microsoft.AspNetCore.Identity;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDB.IdentityUser;

namespace IdentityService
{
    public class ExternalAuthenticationGrant<TUser> : IExtensionGrantValidator where TUser : IdentityUser, new()
    {
        private readonly UserManager<TUser> userManager;
        private readonly IProviderRepository providerRepository;
        private readonly IFacebookAuthProvider facebookAuthProvider;
        private readonly IGoogleAuthProvider googleAuthProvider;

        public ExternalAuthenticationGrant(
            UserManager<TUser> userManager,
            IProviderRepository providerRepository,
            IFacebookAuthProvider facebookAuthProvider = null,
            IGoogleAuthProvider googleAuthProvider = null
            )
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.providerRepository = providerRepository ?? throw new ArgumentNullException(nameof(providerRepository));
            this.facebookAuthProvider = facebookAuthProvider;
            this.googleAuthProvider = googleAuthProvider;

            _providers = new Dictionary<ProviderType, IExternalAuthProvider>();

            if (facebookAuthProvider != null)
                _providers.Add(ProviderType.Facebook, this.facebookAuthProvider);

            if (googleAuthProvider != null)
                _providers.Add(ProviderType.Google, this.googleAuthProvider);
        }

        private readonly Dictionary<ProviderType, IExternalAuthProvider> _providers;

        public string GrantType => "external";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var provider = context.Request.Raw.Get("provider");
            if (string.IsNullOrWhiteSpace(provider))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider");
                return;
            }


            var token = context.Request.Raw.Get("external_token");
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid external token");
                return;
            }

            var providerType = (ProviderType)Enum.Parse(typeof(ProviderType), provider, true);

            if (!Enum.IsDefined(typeof(ProviderType), providerType))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider");
                return;
            }

            var userInfo = _providers[providerType].GetUserInfo(token);

            if (userInfo == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "couldn't retrieve user info from specified provider, please make sure that access token is not expired.");
                return;
            }

            var externalId = userInfo.Value<string>("id");
            if (!string.IsNullOrWhiteSpace(externalId))
            {

                var user = await userManager.FindByLoginAsync(provider, externalId);
                if (null != user)
                {
                    user = await userManager.FindByIdAsync(user.Id);
                    var userClaims = await userManager.GetClaimsAsync(user);
                    context.Result = new GrantValidationResult(user.Id, provider, userClaims, provider, null);
                }
            }
        }
    }
}
