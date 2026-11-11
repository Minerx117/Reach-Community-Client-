namespace ReachCommunityClient.Core.Steam;

/// <summary>Represents a detected local installation of Halo: The Master Chief Collection.</summary>
public sealed record MccInstallation(
    string InstallDirectory,
    string SteamLibraryRoot,
    string AppManifestPath)
{
    public const int SteamAppId = 976730;
    public const int ReachDlcAppId = 1064220;
    public const int ReachMultiplayerContentPackAppId = 1097224;
}
