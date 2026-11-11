namespace ReachCommunityClient.Core.Multiplayer;

/// <summary>Public metadata for a community server. This record contains no player credentials.</summary>
public sealed record CommunityServer(
    string Id,
    string Name,
    string Region,
    string GameMode,
    string MapName,
    int Players,
    int MaxPlayers,
    int PingMilliseconds,
    bool PasswordRequired);

public sealed record ServerBrowserSnapshot(
    DateTimeOffset RetrievedAtUtc,
    IReadOnlyList<CommunityServer> Servers,
    string Source);
