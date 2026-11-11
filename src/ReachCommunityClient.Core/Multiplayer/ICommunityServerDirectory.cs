namespace ReachCommunityClient.Core.Multiplayer;

/// <summary>RCC-owned contract for a future live community server-directory service.</summary>
/// <remarks>No HTTP provider or live endpoint is implemented in v0.01.03.</remarks>
public interface ICommunityServerDirectory
{
    Task<ServerBrowserSnapshot> GetServersAsync(CancellationToken cancellationToken = default);
}
