namespace IdentityService.IdentityServerProviders.Interfaces
{
	public interface IGoogleAuthProvider : IExternalAuthProvider
	{
		Provider Provider { get; }
	}
}