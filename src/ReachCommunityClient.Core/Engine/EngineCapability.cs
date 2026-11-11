namespace ReachCommunityClient.Core.Engine;

public enum EngineCapabilityState
{
    NotImplemented,
    RequiresRccEngineRuntime,
    RequiresRccCommunityService
}

public sealed record EngineCapability(
    string Id,
    string Name,
    EngineCapabilityState State,
    string Details);

/// <summary>Single source of truth for RCC-owned runtime and online-feature readiness.</summary>
public static class EngineCapabilityCatalog
{
    private static readonly IReadOnlyList<EngineCapability> CurrentCapabilities =
    [
        new(
            "reach-main-menu",
            "Original Halo: Reach main menu",
            EngineCapabilityState.RequiresRccEngineRuntime,
            "RCC must implement its own compatible runtime host, renderer, and main-menu scene loading."),
        new(
            "server-browser",
            "Community server browser",
            EngineCapabilityState.RequiresRccCommunityService,
            "RCC must implement a live server-directory API and health/heartbeat reporting."),
        new(
            "multiplayer-sessions",
            "Multiplayer sessions",
            EngineCapabilityState.RequiresRccEngineRuntime,
            "RCC must implement compatible session creation, joining, transport, authentication, and game-state synchronisation."),
        new(
            "dedicated-server",
            "Dedicated server hosting",
            EngineCapabilityState.NotImplemented,
            "An RCC-owned headless runtime and host administration service are future work.")
    ];

    public static IReadOnlyList<EngineCapability> GetCurrent() => CurrentCapabilities;

    public static EngineCapability GetById(string id) =>
        CurrentCapabilities.FirstOrDefault(capability =>
            string.Equals(capability.Id, id, StringComparison.Ordinal))
        ?? throw new KeyNotFoundException($"Unknown engine capability '{id}'.");
}
