namespace ReachCommunityClient.Core.Engine;

/// <summary>Contract for the future RCC-owned native Halo: Reach runtime host.</summary>
/// <remarks>No implementation is provided in v0.01.03. RCC must implement and test its own runtime host.</remarks>
public interface IReachEngineHost
{
    Task<EngineHostStatus> GetStatusAsync(CancellationToken cancellationToken = default);

    Task<EngineHostOperationResult> StartMainMenuAsync(
        EngineHostStartOptions options,
        CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken cancellationToken = default);
}

public sealed record EngineHostStatus(
    bool IsConnected,
    string HostVersion,
    IReadOnlyList<EngineCapability> Capabilities);

public sealed record EngineHostStartOptions(string GameInstallDirectory);

public sealed record EngineHostOperationResult(bool Succeeded, string Message);
