namespace IdentityService.IdentityServerProviders.Interfaces
{
	public interface IFacebookAuthProvider : IExternalAuthProvider
	{
		Provider Provider { get; }
	}
}